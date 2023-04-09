using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using LearnEnglish.XN.Core.ViewModels.Items;
using LearnEnglish.XN.iOS.Extensions;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class MyMessageCell : BaseMessageCell
{
    private UIView _labelView;

    [Export("initWithFrame:")]
    public MyMessageCell(CGRect frame)
        : base(frame)
    {
        InitCell();
    }

    private void InitCell()
    {
        var label = new UILabel
        {
            TextAlignment = UITextAlignment.Center,
            TextColor = UIColor.Black,
            Font = UIFont.SystemFontOfSize(20),
            TranslatesAutoresizingMaskIntoConstraints = false,
            Lines = 0,
            LineBreakMode = UILineBreakMode.WordWrap
        };

        _labelView = new UIView
        {
            BackgroundColor = UIColor.FromRGB(242, 243, 247),
        };
        _labelView.Add(label);

        if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
        {
            _labelView.Layer.CornerRadius = 16;
            _labelView.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MinXMaxYCorner | CACornerMask.MaxXMaxYCorner;
        }
        _labelView.Layer.MasksToBounds = true;

        Add(_labelView);

        NSLayoutConstraint.ActivateConstraints(new []
        {
            // label
            label.BottomAnchor.ConstraintEqualTo(_labelView.BottomAnchor, -10),
            label.TrailingAnchor.ConstraintEqualTo(_labelView.TrailingAnchor, -16),
            label.LeadingAnchor.ConstraintEqualTo(_labelView.LeadingAnchor, 16),
            label.TopAnchor.ConstraintEqualTo(_labelView.TopAnchor, 10),

            // labelView
            _labelView.LeadingAnchor.ConstraintGreaterThanOrEqualTo(LeadingAnchor, 100),
            _labelView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -16),
            _labelView.TopAnchor.ConstraintEqualTo(TopAnchor, 8),
            _labelView.BottomAnchor.ConstraintEqualTo(BottomAnchor, -8),
        });

        this.DelayBind(() =>
        {
            var set = this.CreateBindingSet<MyMessageCell, MessageViewModel>();

            set.Bind(label)
                .For(x => x.Text)
                .To(vm => vm.Text);

            set.Apply();
        });

        this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
        {
            return;
        }

        _labelView.AddCornerRadius(UIRectCorner.TopLeft | UIRectCorner.BottomLeft | UIRectCorner.BottomRight, 16);
    }
}
