using Foundation;
using MvvmCross.Platforms.Ios.Core;
using LearnEnglish.XN.Core;

namespace LearnEnglish.XN.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
    }
}
