using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LearnEnglish.XN.Core.ViewModels.Items;
using LearnEnglish.XN.iOS.Extensions;
using MvvmCross.Base;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.ValueConverters;
using MvvmCross.Platforms.Ios.Binding;
using PropertyChanged;
using Swordfish.NET.Collections.Auxiliary;
using UIKit;

namespace LearnEnglish.XN.iOS.Views;

public class VariantsLayout : UIStackView, IMvxBindingContextOwner, INotifyPropertyChanged
{
    public VariantsLayout()
    {
        this.CreateBindingContext();
        Axis = UILayoutConstraintAxis.Vertical;
        Spacing = 8;
        BackgroundColor = UIColor.Clear;
        Alignment = UIStackViewAlignment.Leading;
        Distribution = UIStackViewDistribution.FillProportionally;
    }

    public nfloat MaxWidth { get; set; }

    [MvxSetToNullAfterBinding]
    [OnChangedMethod(nameof(OnVariantsChanged))]
    public IEnumerable<VariantViewModel> Variants { get; set; }

    public IMvxBindingContext BindingContext { get; set; }

    [MvxSetToNullAfterBinding]
    public object DataContext
    {
        get => BindingContext?.DataContext;
        set
        {
            if (BindingContext == null)
            {
                return;
            }

            BindingContext.DataContext = value;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            DataContext = null;
            BindingContext?.ClearAllBindings();
            BindingContext?.DisposeIfDisposable();
            BindingContext = null;
        }
        base.Dispose(disposing);
    }

    private void OnVariantsChanged()
    {
        if (Variants?.Any() != true)
        {
            return;
        }

        Subviews?.ForEach(view =>
        {
            view?.RemoveFromSuperview();
            view?.Dispose();
        });

        var set = this.CreateBindingSet<VariantsLayout, MessageViewModel>();

        UIStackView horizontalStack = null;

        foreach (var variant in Variants)
        {
            horizontalStack ??= CreateHorizontalStack();

            var button = UIButtonExtensions.CreateUIButton(variant.Text);

            if (MaxWidth >= horizontalStack.Frame.Width + button.Frame.Width + horizontalStack.Spacing)
            {
                horizontalStack.AddArrangedSubview(button);
            }
            else if (horizontalStack.Subviews?.Length == 0)
            {
                horizontalStack.AddArrangedSubview(button);
                AddArrangedSubview(horizontalStack);
                horizontalStack = null;
            }
            else
            {
                AddArrangedSubview(horizontalStack);
                horizontalStack = CreateHorizontalStack();
                horizontalStack.AddArrangedSubview(button);
            }

            set.Bind(button)
                .For(x => x.BindTouchUpInside())
                .To(vm => vm.SelectVariantCommand)
                .WithConversion(new MvxCommandParameterValueConverter(), variant);
        }

        if (horizontalStack != null)
        {
            AddArrangedSubview(horizontalStack);
            horizontalStack = null;
        }

        set.Apply();

        UIStackView CreateHorizontalStack() => new ()
        {
            Axis = UILayoutConstraintAxis.Horizontal,
            BackgroundColor = UIColor.Clear,
            Alignment = UIStackViewAlignment.Center,
            Distribution = UIStackViewDistribution.FillProportionally,
            Spacing = 8,
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
