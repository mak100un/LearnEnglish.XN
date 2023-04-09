using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace LearnEnglish.XN.iOS.Extensions;

public static class UIViewExtensions
{
    public static void AddCornerRadius(this UIView view, UIRectCorner corners, float cornerRadius)
    {
        var path = UIBezierPath.FromRoundedRect(view.Bounds, corners, new CGSize(width: cornerRadius, height: cornerRadius));
        var mask = new CAShapeLayer();
        mask.Path = path.CGPath;
        view.Layer.Mask = mask;
    }
}
