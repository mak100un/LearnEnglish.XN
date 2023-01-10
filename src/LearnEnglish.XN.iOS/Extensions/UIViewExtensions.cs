using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace LearnEnglish.XN.iOS.Extensions;

public static class UIViewExtensions
{
    public static void AddCornerRadius(this UIView view, UIRectCorner rectCorner, CGSize size)
    {
        using var path = UIBezierPath.FromRoundedRect(view.Bounds, rectCorner, size);
        using var maskLayer = new CAShapeLayer
        {
            Path = path.CGPath,
        };

        view.Layer.Mask = maskLayer;
    }
}
