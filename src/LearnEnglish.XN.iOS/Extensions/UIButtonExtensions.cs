using CoreAnimation;
using CoreGraphics;
using LearnEnglish.XN.iOS.Views;
using UIKit;

namespace LearnEnglish.XN.iOS.Extensions;

public static class UIButtonExtensions
{
    public static UIButton CreateUIButton(string text)
    {
        using var buttonConfig = UIButtonConfiguration.PlainButtonConfiguration;
        buttonConfig.TitleAlignment = UIButtonConfigurationTitleAlignment.Center;
        buttonConfig.BaseForegroundColor = UIColor.Black;
        buttonConfig.ContentInsets = new NSDirectionalEdgeInsets(10, 20, 10, 20);
        var button = new PrimaryButton
        {
            HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
            Configuration = buttonConfig,
        };
        button.BackgroundColor = UIColor.White;
        button.SetTitle(text, UIControlState.Normal);
        button.Layer.CornerRadius = 16;
        button.Layer.MasksToBounds = false;
        button.Layer.ShadowRadius = button.Layer.CornerRadius;
        button.Layer.ShadowOpacity = 0.2f;
        button.Layer.ShadowOffset = new CGSize(width: 2.0, height: 2.0);
        button.Layer.ShadowColor = UIColor.DarkGray.CGColor;
        return button;
    }
}
