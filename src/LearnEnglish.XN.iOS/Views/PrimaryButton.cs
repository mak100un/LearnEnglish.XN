using UIKit;

namespace LearnEnglish.XN.iOS.Views;

public class PrimaryButton : UIButton
{
    public override bool Enabled
    {
        get => base.Enabled;
        set
        {
            base.Enabled = value;
            Alpha = !value ? 0.7f : 1f;
            BackgroundColor = UIColor.White;
        }
    }
}
