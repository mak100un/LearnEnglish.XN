using CoreGraphics;
using Foundation;
using LearnEnglish.XN.iOS.Views;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class BaseCollectionViewCell : UICollectionViewCell
{
    [Export("initWithFrame:")]
    protected BaseCollectionViewCell(CGRect frame)
        : base(frame)
    {
        BackgroundColor = UIColor.Clear;
        //WidthAnchor.ConstraintEqualTo(UIScreen.MainScreen.Bounds.Width).Active = true;
    }

    public override UICollectionViewLayoutAttributes PreferredLayoutAttributesFittingAttributes(UICollectionViewLayoutAttributes layoutAttributes)
    {
        var newLayoutAttributes = base.PreferredLayoutAttributesFittingAttributes(layoutAttributes);
        if (newLayoutAttributes.Bounds.Size.Width != UIScreen.MainScreen.Bounds.Width)
        {
            return newLayoutAttributes;
        }

        (Superview as MessagesCollectionView)?.TrySetItemSize(newLayoutAttributes.IndexPath, newLayoutAttributes.Bounds.Size);

        return newLayoutAttributes;
    }
}
