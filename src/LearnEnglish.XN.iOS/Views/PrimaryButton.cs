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
            Alpha = !value ? 0.8f : 1f;
        }
    }
}
