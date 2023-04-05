using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class LoaderCell : UICollectionViewCell
{
    [Export("initWithFrame:")]
    public LoaderCell(CGRect frame)
        : base(frame)
    {
        BackgroundColor = UIColor.Clear;
        InitCell();
    }

    private void InitCell()
    {
        var indicator = new UIActivityIndicatorView { Color = UIColor.White };
        ContentView.Add(indicator);

        NSLayoutConstraint.ActivateConstraints(new []
        {
            // indicator
            indicator.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor),
            indicator.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor),
            indicator.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 16),
            indicator.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor, -8),
        });

        ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }
}
