using Foundation;
using MvvmCross.Platforms.Ios.Core;
using LearnEnglish.XN.Core;
using LearnEnglish.XN.iOS.Extensions;
using LearnEnglish.XN.iOS.ViewControllers;
using UIKit;

namespace LearnEnglish.XN.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        public override void DidEnterBackground(UIApplication application)
        {
            base.DidEnterBackground(application);
            (UIApplication.SharedApplication?.KeyWindow?.RootViewController?.GetPresentedViewController() as LaunchViewController)?.PauseMusic();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            base.WillEnterForeground(application);
            (UIApplication.SharedApplication?.KeyWindow?.RootViewController?.GetPresentedViewController() as LaunchViewController)?.PlayMusic();
        }
    }
}
