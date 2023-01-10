using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LearnEnglish.XN.Core.ViewModels.Items;
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

            using var userNameButtonConfig = UIButtonConfiguration.FilledButtonConfiguration;
            userNameButtonConfig.TitleAlignment = UIButtonConfigurationTitleAlignment.Center;
            userNameButtonConfig.Title = variant.Text;
            var button = new UIButton
            {
                HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
                BackgroundColor = UIColor.White,
                Configuration = userNameButtonConfig,
            };
            button.Layer.CornerRadius = 16;
            button.SetTitleColor(UIColor.Black, UIControlState.Normal);
            button.ContentEdgeInsets = new UIEdgeInsets(6, 16, 6, 16);

            if (MaxWidth >= horizontalStack.Frame.Width + button.Frame.Width + horizontalStack.Spacing)
            {
                horizontalStack.Add(button);
            }
            else if (horizontalStack.Subviews?.Length == 0)
            {
                horizontalStack.Add(button);
                Add(horizontalStack);
                horizontalStack = null;
            }
            else
            {
                Add(horizontalStack);
                horizontalStack = CreateHorizontalStack();
                horizontalStack.Add(button);
            }

            set.Bind(button)
                .For(x => x.BindTouchUpInside())
                .To(vm => vm.SelectVariantCommand)
                .WithConversion(new MvxCommandParameterValueConverter(), variant);
        }

        if (horizontalStack != null)
        {
            Add(horizontalStack);
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
