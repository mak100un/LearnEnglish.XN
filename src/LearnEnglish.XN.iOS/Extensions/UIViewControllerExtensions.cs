using UIKit;

namespace LearnEnglish.XN.iOS.Extensions;

public static class UIViewControllerExtensions
{
    public static UIViewController GetPresentedViewController(this UIViewController viewController) =>
        viewController switch
        {
            UINavigationController navigationController => GetPresentedViewController(navigationController.VisibleViewController),
            not null when viewController.PresentedViewController != null => GetPresentedViewController(viewController.PresentedViewController),
            _ => viewController
        };
}
