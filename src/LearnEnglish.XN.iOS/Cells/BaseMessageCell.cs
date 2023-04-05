using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Base;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public abstract class BaseMessageCell : UICollectionViewCell, IMvxBindingContextOwner
{
    public override CGSize SystemLayoutSizeFittingSize(CGSize size) => ContentView.SystemLayoutSizeFittingSize(new CGSize(UIScreen.MainScreen.Bounds.Width, 1));


    [Export("initWithFrame:")]
    protected BaseMessageCell(CGRect frame)
        : base(frame)
    {
        this.CreateBindingContext();
        BackgroundColor = UIColor.Green;
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
