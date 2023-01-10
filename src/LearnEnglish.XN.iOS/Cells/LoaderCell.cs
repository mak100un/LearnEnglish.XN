using CoreGraphics;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class LoaderCell : UICollectionViewCell
{
    public LoaderCell(CGRect frame)
        : base(frame)
    {
        BackgroundColor = UIColor.Clear;
        InitCell();
    }

    private void InitCell()
    {
        var indicator = new UIActivityIndicatorView { Color = UIColor.White };
        indicator.LayoutMargins = new UIEdgeInsets(16, 0, 8, 0);
        Add(indicator);

        NSLayoutConstraint.ActivateConstraints(new []
        {
            // indicator
            indicator.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
            indicator.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
            indicator.TopAnchor.ConstraintEqualTo(TopAnchor),
            indicator.BottomAnchor.ConstraintEqualTo(BottomAnchor),
        });
    }
}
