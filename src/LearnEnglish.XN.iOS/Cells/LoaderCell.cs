using Cirrious.FluentLayouts.Touch;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class LoaderCell : BaseCollectionViewCell
{
    private UIActivityIndicatorView _indicator;

    [Export("initWithFrame:")]
    public LoaderCell(CGRect frame)
        : base(frame)
    {
        InitCell();
    }

    private void InitCell()
    {
        _indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Medium)
        {
            Color = UIColor.White,
            HidesWhenStopped = false,
            Transform = CGAffineTransform.MakeScale(1.5f, 1.5f)
        };
        Add(_indicator);

        StartAnimating();

        NSLayoutConstraint.ActivateConstraints(new []
        {
            // indicator
            _indicator.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
            _indicator.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
            _indicator.TopAnchor.ConstraintEqualTo(TopAnchor, 16),
            _indicator.BottomAnchor.ConstraintEqualTo(BottomAnchor, -8),
        });

        this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    public void StartAnimating() => DispatchQueue.MainQueue.DispatchAsync(() => _indicator?.StartAnimating());

    protected override void Dispose(bool disposing)
    {
        _indicator?.Dispose();
        _indicator = null;
        base.Dispose(disposing);
    }
}
