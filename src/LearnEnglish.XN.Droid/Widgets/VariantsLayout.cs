using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Google.Flexbox;
using LearnEnglish.XN.Core.ViewModels.Items;
using LearnEnglish.XN.Droid.Extensions;
using MvvmCross.Base;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.ValueConverters;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using PropertyChanged;

namespace LearnEnglish.XN.Droid.Widgets;

public class VariantsLayout : FlexboxLayout, IMvxBindingContextOwner, INotifyPropertyChanged
{
    protected VariantsLayout(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public VariantsLayout(Context context)
        : base(context)
    {
        BindingContext = new MvxAndroidBindingContext(context, (IMvxLayoutInflaterHolder)context);
    }

    public VariantsLayout(Context context, IAttributeSet attrs)
        : base(context, attrs)
    {
        BindingContext = new MvxAndroidBindingContext(Context, (IMvxLayoutInflaterHolder)Context);
    }

    public VariantsLayout(Context context, IAttributeSet attrs, int defStyleAttr)
        : base(context, attrs, defStyleAttr)
    {
    }

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
        base.Dispose(disposing);
        DataContext = null;
        BindingContext?.ClearAllBindings();
        BindingContext?.DisposeIfDisposable();
        BindingContext = null;
    }

    private void OnVariantsChanged()
    {
        if (Variants?.Any() != true)
        {
            return;
        }

        RemoveAllViews();
        var set = this.CreateBindingSet<VariantsLayout, MessageViewModel>();

        var horizontalPadding = Context.ToPixels(16);
        var verticalPadding = Context.ToPixels(6);

        using var layoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
        var margin = Context.ToPixels(8);
        layoutParams.SetMargins(0, 0,margin,margin);

        foreach (var variant in Variants)
        {
            var button = new Button(Context)
            {
                TextAlignment = TextAlignment.Center,
                Text = variant.Text,
            };
            button.SetBackgroundResource(Resource.Drawable.variant_item_background);
            button.SetPadding(verticalPadding, horizontalPadding, verticalPadding, horizontalPadding);
            set.Bind(button)
                .For(x => x.BindClick())
                .To(vm => vm.SelectVariantCommand)
                .WithConversion(new MvxCommandParameterValueConverter(), variant);

            AddView(button, layoutParams);
        }

        set.Apply();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
