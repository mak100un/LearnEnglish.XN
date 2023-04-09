using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using LearnEnglish.XN.Core.Definitions.Extensions;
using LearnEnglish.XN.Core.Services.Interfaces;
using MvvmCross.Platforms.Android;
using Xamarin.Essentials;

namespace LearnEnglish.XN.Droid.Services;

public class DialogService : IDialogService
{
    private readonly IMvxAndroidCurrentTopActivity _topActivity;
    public DialogService(IMvxAndroidCurrentTopActivity topActivity) => _topActivity = topActivity;

    public Task<string> DisplayInputAsync(string title, string text, string previousText, string accept, string cancel)
    {
        var alertDialog = new AlertDialog.Builder(_topActivity.Activity)
            .SetTitle(title)
            ?.SetMessage(text)
            ?.Create();

        if (alertDialog == null)
        {
            return Task.FromResult(default(string));
        }

        var tcs = new TaskCompletionSource<string>();

        var frameLayout = new FrameLayout(_topActivity.Activity);


        var margin = (int)(22 * _topActivity.Activity.Resources.DisplayMetrics.Density);
        using var layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
        {
            LeftMargin = margin,
            RightMargin = margin
        };

        var editText = new EditText(_topActivity.Activity)
        {
            Text = previousText,
            Hint = "Введите текст",
            LayoutParameters = layoutParams,
        };

        frameLayout.AddView(editText);
        alertDialog.SetView(frameLayout);

        alertDialog.SetButton((int)DialogButtonType.Positive, accept, (_, _) =>
        {
            tcs.TrySetResult(editText.Text);
            Dispose();
        });
        alertDialog.SetButton((int)DialogButtonType.Negative, cancel, (_, _) =>
        {
            tcs.TrySetResult(null);
            Dispose();
        });
        alertDialog.SetCanceledOnTouchOutside(true);
        alertDialog.SetCancelable(true);
        alertDialog.CancelEvent += OnCancelEvent;

        alertDialog.Window?.SetSoftInputMode(SoftInput.StateVisible);
        alertDialog.Show();
        editText.RequestFocus();

        return tcs.Task;

        void OnCancelEvent(object sender, EventArgs e)
        {
            tcs.TrySetResult(null);
            Dispose();
        }

        void Dispose()
        {
            alertDialog?.Then(dialog => dialog.CancelEvent -= OnCancelEvent);
            alertDialog?.Dispose();
            alertDialog = null;
        }
    }

    public Task DisplaySuccessMessageAsync() => Task.CompletedTask;

    public void ShowToast(string text) => Toast.MakeText(_topActivity.Activity, text, ToastLength.Short)?.Show();
}
