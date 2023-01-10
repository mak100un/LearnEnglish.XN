using System;
using Android.Media;
using LearnEnglish.XN.Core.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace LearnEnglish.XN.Droid.Fragments;

[MvxFragmentPresentation(IsCacheableFragment = true, ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.content_frame, AddToBackStack = false)]
public class LaunchFragment : BaseFragment<LaunchViewModel>
{
    private MediaPlayer _mediaPlayer;
    protected override int ResourceId => Resource.Layout.launch_layout;
    protected override bool ShowHomeEnabled => false;

    public override void OnResume()
    {
        base.OnResume();
        try
        {
            if (_mediaPlayer == null)
            {
                _mediaPlayer = MediaPlayer.Create(Activity, Resource.Raw.train);
                _mediaPlayer.Looping = true;
            }
            _mediaPlayer?.Start();
        }
        catch (Exception e)
        {
            ViewModel.Logger.LogError(e, e.Message);
        }
    }

    public override void OnPause()
    {
        base.OnPause();
        _mediaPlayer?.Pause();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _mediaPlayer?.Stop();
        _mediaPlayer?.Dispose();
        _mediaPlayer = null;
    }
}
