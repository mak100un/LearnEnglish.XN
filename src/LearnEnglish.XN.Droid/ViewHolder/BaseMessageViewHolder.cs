using System;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Base;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;

namespace LearnEnglish.XN.Droid.ViewHolder;

public abstract class BaseMessageViewHolder : RecyclerView.ViewHolder, IMvxBindingContextOwner
{
    protected BaseMessageViewHolder(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    protected BaseMessageViewHolder(View itemView, IMvxBindingContext context)
        : base(itemView)
    {
        BindingContext = context;
    }

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
}
