using UIKit;

namespace LearnEnglish.XN.iOS.Extensions;

public static class ViewControllerExtensions
{
    public static UIViewController GetPresentedViewController(this UIViewController viewController) =>
        viewController switch
        {
            UINavigationController navigationController => GetPresentedViewController(navigationController.VisibleViewController),
            not null when viewController.PresentedViewController != null => GetPresentedViewController(viewController.PresentedViewController),
            _ => viewController
        };
}
