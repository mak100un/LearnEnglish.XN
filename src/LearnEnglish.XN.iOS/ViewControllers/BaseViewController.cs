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
        NavigationItem.Title = "LearnEnglish.XN";
        NavigationItem.BackButtonTitle = "Back";
        NavigationController.NavigationBar.BarStyle = UIBarStyle.Default;
        NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes
        {
            ForegroundColor = UIColor.Black,
        };

        NavigationController.NavigationBar.BackgroundColor = UIColor.White;
        NavigationController.NavigationBar.TintColor = UIColor.Black;

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
