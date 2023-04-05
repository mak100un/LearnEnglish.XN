using System;
using AVFoundation;
using Cirrious.FluentLayouts.Touch;
using FFImageLoading.Cross;
using LearnEnglish.XN.Core.Definitions.Enums;
using LearnEnglish.XN.Core.ViewModels;
using LearnEnglish.XN.iOS.Extensions;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.ValueConverters;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewControllers;

[MvxRootPresentation(WrapInNavigationController = true )]
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
            LineBreakMode = UILineBreakMode.TailTruncation,
            Lines = 1,
        });

        View.Add(_image = new MvxCachedImageView
        {
            ContentMode = UIViewContentMode.ScaleAspectFit,
            ImagePath = "res:new_year.gif",
        });

        View.Add(_stackContainer = new UIView());

        _stackContainer.Add(_stack = new UIStackView
        {
            Alignment = UIStackViewAlignment.Center,
            Axis = UILayoutConstraintAxis.Vertical,
            Spacing = 20,
            TranslatesAutoresizingMaskIntoConstraints = false,
        });

        _stack.AddArrangedSubview(_userNameButton = UIButtonExtensions.CreateUIButton("Задать имя"));

        _stack.AddArrangedSubview(_chatButton = UIButtonExtensions.CreateUIButton("Чат"));
    }

    protected override void LayoutView()
    {
        base.LayoutView();
        var safeAreaGuide = View.SafeAreaLayoutGuide;

        // _backgroundImage
        NSLayoutConstraint.ActivateConstraints(new []
        {
            _backgroundImage.BottomAnchor.ConstraintEqualTo(safeAreaGuide.BottomAnchor),
            _backgroundImage.TopAnchor.ConstraintEqualTo(TopLayoutGuide.GetBottomAnchor()),
            _backgroundImage.LeadingAnchor.ConstraintEqualTo(safeAreaGuide.LeadingAnchor),
            _backgroundImage.TrailingAnchor.ConstraintEqualTo(safeAreaGuide.TrailingAnchor),

            _image.HeightAnchor.ConstraintLessThanOrEqualTo(200),
            _image.WidthAnchor.ConstraintLessThanOrEqualTo(200),
            _image.HeightAnchor.ConstraintEqualTo(200),
            _stack.TopAnchor.ConstraintGreaterThanOrEqualTo(_stackContainer.TopAnchor),
            _stack.BottomAnchor.ConstraintLessThanOrEqualTo(_stackContainer.BottomAnchor),

            _stackContainer.TopAnchor.ConstraintGreaterThanOrEqualTo(_image.BottomAnchor),
        });

        View.AddConstraints(
            // _label
            _label.AtTopOf(_backgroundImage, 6),
            _label.AtLeadingOf(_backgroundImage, 6),
            _label.AtTrailingOf(_backgroundImage, 6),

            // _image
            _image.Below(_label, 150),
            _image.AtLeadingOf(_backgroundImage, 50),
            _image.AtTrailingOf(_backgroundImage, 50),

            // _stackContainer
            _stackContainer.AtLeadingOf(_backgroundImage, 16),
            _stackContainer.AtTrailingOf(_backgroundImage, 16),
            _stackContainer.AtBottomOf(_backgroundImage, 40),

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
           // _player?.Play();
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

