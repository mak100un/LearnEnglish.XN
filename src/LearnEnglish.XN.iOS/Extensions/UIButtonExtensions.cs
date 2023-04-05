using UIKit;

namespace LearnEnglish.XN.iOS.Extensions;

public static class UIButtonExtensions
{
    public static UIButton CreateUIButton(string text)
    {
        using var buttonConfig = UIButtonConfiguration.FilledButtonConfiguration;
        buttonConfig.TitleAlignment = UIButtonConfigurationTitleAlignment.Center;
        buttonConfig.BaseBackgroundColor = UIColor.White;
        buttonConfig.BaseForegroundColor = UIColor.Black;
        buttonConfig.ContentInsets = new NSDirectionalEdgeInsets(6, 16, 6, 16);
        var button = new UIButton
        {
            HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
            Configuration = buttonConfig,
            ClipsToBounds = true,
        };
        button.SetTitle(text, UIControlState.Normal);
        button.Layer.CornerRadius = 16;
        return button;
    }
}
