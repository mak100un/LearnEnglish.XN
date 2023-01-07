using Cirrious.FluentLayouts.Touch;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewControllers;

public abstract class BaseViewController<TViewModel> : MvxViewController<TViewModel>
    where TViewModel : class, IMvxViewModel
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        View.BackgroundColor = UIColor.White;
        CreateView();
        LayoutView();
        BindView();
    }

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);
        View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    protected virtual void CreateView()
    {
    }

    protected virtual void LayoutView()
    {
    }

    protected virtual void BindView()
    {
    }
}
