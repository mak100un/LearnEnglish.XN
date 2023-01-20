using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using LearnEnglish.XN.Core.Definitions.Extensions;
using LearnEnglish.XN.Core.ViewModels;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace LearnEnglish.XN.Droid
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme", Label = "LearnEnglish.XN", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : MvxActivity<MainViewModel>
    {
        private Toolbar _toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            (_toolbar = FindViewById<Toolbar>(Resource.Id.toolbar))?.Then(toolbar =>
            {
                SetActionBar(toolbar);
                toolbar.NavigationOnClick += OnNavigationClick;
            });

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            _toolbar?.Then(toolbar => toolbar.NavigationOnClick -= OnNavigationClick);
            base.OnDestroy();
        }

        private void OnNavigationClick(object sender, EventArgs e)
        {
            OnBackPressed();
        }
    }
}
