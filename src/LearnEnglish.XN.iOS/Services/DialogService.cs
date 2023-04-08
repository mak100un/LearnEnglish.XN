using System;
using System.Drawing;
using System.Threading.Tasks;
using Foundation;
using LearnEnglish.XN.Core.Services.Interfaces;
using UIKit;

namespace LearnEnglish.XN.iOS.Services
{
    public class DialogService : IDialogService
    {
        public Task<string> DisplayInputAsync(string title, string message, string previousText, string accept, string cancel)
        {
            var tcs = new TaskCompletionSource<string>();

            var currentViewController = GetPresentedViewController(UIApplication.SharedApplication.KeyWindow.RootViewController);

            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.View.Superview.UserInteractionEnabled = true;
            alert.View.Superview.AddGestureRecognizer(new UITapGestureRecognizer(Dispose));
            alert.AddTextField(uiTextField =>
            {
                uiTextField.Placeholder = "Введите текст";
                uiTextField.KeyboardType = UIKeyboardType.Default;
                uiTextField.Text = previousText;
            });
            var oldFrame = alert.View.Frame;
            alert.View.Frame = new RectangleF((float)oldFrame.X, (float)oldFrame.Y, (float)oldFrame.Width, (float)oldFrame.Height - 10 * 2);

            alert.AddAction(UIAlertAction.Create(cancel, UIAlertActionStyle.Cancel, a =>
            {
                tcs.TrySetResult(null);
                Dispose();
            }));

            alert.AddAction(UIAlertAction.Create(accept, UIAlertActionStyle.Default,
            a =>
            {
                tcs.TrySetResult(alert.TextFields[0].Text);
                Dispose();
            }));

            currentViewController.PresentViewController(alert, true, null);

            return tcs.Task;

            void Dispose()
            {
                alert?.DismissViewController(true, null);
                alert?.Dispose();
                alert = null;
            }
        }

        public Task DisplaySuccessMessageAsync()=> Task.CompletedTask;

        public void ShowToast(string text)
        {
            var window = new UIWindow
            {
                BackgroundColor = UIColor.Clear,
                RootViewController = new UIViewController(),
                WindowLevel = UIWindowLevel.Alert + 1,
            };

            window.RootViewController.View.BackgroundColor = UIColor.Clear;
            window.MakeKeyAndVisible();

            var alert = UIAlertController.Create(null, text, UIAlertControllerStyle.Alert);

            NSTimer.CreateScheduledTimer(2.0, (obj) => DismissMessage(alert, obj, null));

            var attributedString = new NSAttributedString(text, foregroundColor: UIColor.Black);
            alert.SetValueForKey(attributedString, new NSString("attributedMessage"));

            if(!UIDevice.CurrentDevice.CheckSystemVersion(9,0))
            {
                // For iOS 8, we need to explicitly set the size of the window
                window.Frame = new RectangleF(0, 0, (float)UIScreen.MainScreen.Bounds.Width, (float)UIScreen.MainScreen.Bounds.Height);
            }

            window.RootViewController.PresentViewController(alert, true, null);

            void DismissMessage(UIAlertController alert, NSTimer alertDelay, Action complete)
            {
                alert?.DismissViewController(true, complete);
                alert?.Dispose();
                alertDelay?.Dispose();
                alert = null;
            }
        }

        private UIViewController GetPresentedViewController(UIViewController viewController) =>
            viewController switch
            {
                UINavigationController navigationController => GetPresentedViewController(navigationController.VisibleViewController),
                not null when viewController.PresentedViewController != null => GetPresentedViewController(viewController.PresentedViewController),
                _ => viewController
            };
    }
}
