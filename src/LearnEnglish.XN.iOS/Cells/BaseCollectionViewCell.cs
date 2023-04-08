using CoreGraphics;
using Foundation;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class BaseCollectionViewCell : UICollectionViewCell
{
    [Export("initWithFrame:")]
    protected BaseCollectionViewCell(CGRect frame)
        : base(frame)
    {
        BackgroundColor = UIColor.Clear;
    }
}
