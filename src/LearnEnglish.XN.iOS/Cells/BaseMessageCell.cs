using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Base;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public abstract class BaseMessageCell : BaseCollectionViewCell, IMvxBindingContextOwner
{
    [Export("initWithFrame:")]
    protected BaseMessageCell(CGRect frame)
        : base(frame) => this.CreateBindingContext();

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
