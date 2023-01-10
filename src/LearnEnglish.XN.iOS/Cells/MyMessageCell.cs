using CoreGraphics;
using LearnEnglish.XN.Core.ViewModels.Items;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class MyMessageCell : BaseMessageCell
{
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
            LayoutMargins = new UIEdgeInsets(10, 16, 10, 16)
        };
        var labelView = new UIView
        {
            BackgroundColor = UIColor.FromRGB(242, 243, 247),
            LayoutMargins = new UIEdgeInsets(8, 0, 8, 0),
        };
        labelView.Add(label);
        labelView.Layer.CornerRadius = 16;
        labelView.Layer.MasksToBounds = true;

        Add(labelView);

        NSLayoutConstraint.ActivateConstraints(new []
        {
            // label
            label.BottomAnchor.ConstraintEqualTo(labelView.BottomAnchor),
            label.TrailingAnchor.ConstraintEqualTo(labelView.TrailingAnchor),
            label.LeadingAnchor.ConstraintEqualTo(labelView.LeadingAnchor),
            label.TopAnchor.ConstraintEqualTo(labelView.TopAnchor),

            // labelView
            labelView.LeadingAnchor.ConstraintGreaterThanOrEqualTo(LeadingAnchor, 100),
            labelView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -16),
            labelView.TopAnchor.ConstraintEqualTo(TopAnchor),
            labelView.BottomAnchor.ConstraintEqualTo(BottomAnchor),
        });

        this.DelayBind(() =>
        {
            var set = this.CreateBindingSet<MyMessageCell, MessageViewModel>();

            set.Bind(label)
                .For(x => x.Text)
                .To(vm => vm.Text);

            set.Apply();
        });
    }
}
