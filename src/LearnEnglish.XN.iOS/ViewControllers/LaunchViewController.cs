using System;
using AVFoundation;
using Cirrious.FluentLayouts.Touch;
using FFImageLoading.Cross;
using LearnEnglish.XN.Core.Definitions.Enums;
using LearnEnglish.XN.Core.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.ValueConverters;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewControllers;

[MvxRootPresentation(WrapInNavigationController = true)]
public class LaunchViewController : BaseViewController<LaunchViewModel>
{
    private AVAudioPlayer _player;
    private UIImageView _backgroundImage;
    private UILabel _label;
    private MvxCachedImageView _image;
    private UIView _stackContainer;
    private UIStackView _stack;
    private UIButton _chatButton, _userNameButton;

    public override void ViewDidDisappear(bool animated)
    {
        base.ViewDidDisappear(animated);
        _player?.Pause();
    }

    protected override void CreateView()
    {
        base.CreateView();

        View.Add(_backgroundImage = new UIImageView
        {
            Image = new UIImage("background_img.jpeg"),
            ContentMode = UIViewContentMode.ScaleAspectFill,
        });

        View.Add(_label = new UILabel
        {
            TextAlignment = UITextAlignment.Right,
            TextColor = UIColor.White,
            Font = UIFont.SystemFontOfSize(20),
        });

        View.Add(_image = new MvxCachedImageView
        {
            ContentMode = UIViewContentMode.ScaleAspectFit,
        });

        View.Add(_stackContainer = new UIView());

        _stackContainer.Add(_stack = new UIStackView
        {
            Alignment = UIStackViewAlignment.Center,
            Axis = UILayoutConstraintAxis.Vertical,
            Spacing = 20,
        });

        using var userNameButtonConfig = UIButtonConfiguration.FilledButtonConfiguration;
        userNameButtonConfig.TitleAlignment = UIButtonConfigurationTitleAlignment.Center;
        userNameButtonConfig.Title = "Задать имя";
        _userNameButton = new UIButton
        {
            HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
            BackgroundColor = UIColor.White,
            Configuration = userNameButtonConfig,
        };
        _userNameButton.Layer.CornerRadius = 16;
        _userNameButton.SetTitleColor(UIColor.Black, UIControlState.Normal);

        _stack.Add(_userNameButton);

        using var chatButtonConfig = UIButtonConfiguration.FilledButtonConfiguration;
        chatButtonConfig.TitleAlignment = UIButtonConfigurationTitleAlignment.Center;
        chatButtonConfig.Title = "Чат";
        _chatButton = new UIButton
        {
            HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
            BackgroundColor = UIColor.White,
            Configuration = chatButtonConfig,
        };
        _chatButton.Layer.CornerRadius = 16;
        _chatButton.SetTitleColor(UIColor.Black, UIControlState.Normal);

        _stack.Add(_chatButton);
    }

    protected override void LayoutView()
    {
        base.LayoutView();
        var safeAreaGuide = View.SafeAreaLayoutGuide;

        // _backgroundImage
        NSLayoutConstraint.ActivateConstraints(new []
        {
            _backgroundImage.BottomAnchor.ConstraintEqualTo(safeAreaGuide.BottomAnchor),
            _backgroundImage.TopAnchor.ConstraintEqualTo(NavigationController.NavigationBar.BottomAnchor),
            _backgroundImage.LeadingAnchor.ConstraintEqualTo(safeAreaGuide.LeadingAnchor),
            _backgroundImage.TrailingAnchor.ConstraintEqualTo(safeAreaGuide.TrailingAnchor),
            _image.HeightAnchor.ConstraintEqualTo(200),
            _stack.TopAnchor.ConstraintGreaterThanOrEqualTo(_stackContainer.TopAnchor),
            _stack.BottomAnchor.ConstraintLessThanOrEqualTo(_stackContainer.BottomAnchor),
        });

        View.AddConstraints(
            // _label
            _label.AtTopOf(_backgroundImage, 6),
            _label.AtLeadingOf(_backgroundImage, 6),
            _label.AtTrailingOf(_backgroundImage, -6),

            // _image
            _image.Below(_label, 156),
            _image.AtLeadingOf(_backgroundImage, 50),
            _image.AtTrailingOf(_backgroundImage, -50),

            // _stackContainer
            _stackContainer.Below(_image),
            _stackContainer.AtLeadingOf(_backgroundImage, 16),
            _stackContainer.AtTrailingOf(_backgroundImage, -16),
            _stackContainer.AtBottomOf(_backgroundImage, -40),

            // _stack
            _stack.WithSameLeading(_stackContainer),
            _stack.WithSameTrailing(_stackContainer),
            _stack.WithSameCenterY(_stackContainer)
            );
    }

    protected override void BindView()
    {
        base.BindView();

        var set = this.CreateBindingSet<LaunchViewController, LaunchViewModel>();

        set
            .Bind(_image)
            .For(v => v.ImagePath)
            .To(vm => "res:cola.gif");

        set
            .Bind(_userNameButton)
            .For(v => v.BindTouchUpInside())
            .To(vm => vm.ActionCommand)
            .WithConversion(new MvxCommandParameterValueConverter(), LaunchActionTypes.UserName);

        set
            .Bind(_chatButton)
            .For(v => v.BindTouchUpInside())
            .To(vm => vm.ActionCommand)
            .WithConversion(new MvxCommandParameterValueConverter(), LaunchActionTypes.Chat);

        set
            .Bind(_label)
            .For(v => v.Text)
            .To(vm => vm.UserName);

        set.Apply();

        try
        {
            if (_player == null)
            {
                var url = NibBundle?.GetUrlForResource("new_year", "mp3");
                AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                AVAudioSession.SharedInstance().SetActive(true);
                _player = AVAudioPlayer.FromUrl(url);
                _player.NumberOfLoops = -1;
            }
            _player?.Play();
        }
        catch (Exception e)
        {
            ViewModel.Logger.LogError(e, e.Message);
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _player?.Stop();
        _player?.Dispose();
        _player = null;
    }
}

