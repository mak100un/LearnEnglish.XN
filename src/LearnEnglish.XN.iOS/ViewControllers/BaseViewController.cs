using Cirrious.FluentLayouts.Touch;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewControllers;

public abstract class BaseViewController<TViewModel> : MvxViewController<TViewModel>
    where TViewModel : class, IMvxViewModel
{
    public sealed override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View.BackgroundColor = UIColor.White;
        NavigationController.Title = "LearnEnglish.XN";
        NavigationController.HidesBarsOnSwipe = true;
        NavigationController.NavigationItem.BackButtonTitle = "Back";
        NavigationController.NavigationBar.BarStyle = UIBarStyle.Default;
        NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes
        {
            ForegroundColor = UIColor.Black,
        };
        NavigationController.NavigationBar.Translucent = false;
        NavigationController.NavigationBar.ShadowImage = null;
        NavigationController.NavigationBar.Hidden = false;
        NavigationController.NavigationBar.BarTintColor = UIColor.White;
        NavigationController.NavigationBar.TintColor = UIColor.Black;
        NavigationController.SetNeedsStatusBarAppearanceUpdate();
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
