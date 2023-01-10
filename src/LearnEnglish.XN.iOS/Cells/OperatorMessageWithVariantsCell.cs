using CoreGraphics;
using LearnEnglish.XN.Core.ViewModels.Items;
using LearnEnglish.XN.iOS.Views;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class OperatorMessageWithVariantsCell : BaseMessageCell
{
    public OperatorMessageWithVariantsCell(CGRect frame)
        : base(frame)
    {
        InitCell();
    }

    private void InitCell()
    {
        var label = new UILabel
        {
            TextAlignment = UITextAlignment.Center,
            TextColor = UIColor.White,
            Font = UIFont.SystemFontOfSize(20),
            LayoutMargins = new UIEdgeInsets(10, 16, 10, 16)
        };
        var labelView = new UIView
        {
            BackgroundColor = UIColor.FromRGB(145, 215, 255),
            LayoutMargins = new UIEdgeInsets(8, 0, 8, 0),
        };
        labelView.Add(label);
        labelView.Layer.CornerRadius = 16;
        labelView.Layer.MasksToBounds = true;

        Add(labelView);

        var variantsLayout = new VariantsLayout
        {
            MaxWidth = Bounds.Width - 100f - 16f,
        };

        Add(variantsLayout);

        NSLayoutConstraint.ActivateConstraints(new []
        {
            // label
            label.BottomAnchor.ConstraintEqualTo(labelView.BottomAnchor),
            label.TrailingAnchor.ConstraintEqualTo(labelView.TrailingAnchor),
            label.LeadingAnchor.ConstraintEqualTo(labelView.LeadingAnchor),
            label.TopAnchor.ConstraintEqualTo(labelView.TopAnchor),

            // labelView
            labelView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 16),
            labelView.TrailingAnchor.ConstraintLessThanOrEqualTo(TrailingAnchor, -100),
            labelView.TopAnchor.ConstraintEqualTo(TopAnchor),

            // variantsLayout
            variantsLayout.TopAnchor.ConstraintEqualTo(labelView.BottomAnchor),
            variantsLayout.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 16),
            variantsLayout.TrailingAnchor.ConstraintLessThanOrEqualTo(TrailingAnchor, -100),
            variantsLayout.BottomAnchor.ConstraintEqualTo(BottomAnchor, -8),
        });

        this.DelayBind(() =>
        {
            var set = this.CreateBindingSet<OperatorMessageWithVariantsCell, MessageViewModel>();

            set.Bind(label)
                .For(x => x.Text)
                .To(vm => vm.Text);

            set.Bind(variantsLayout)
                .For(x => x.DataContext)
                .To(vm => vm);

            set.Bind(variantsLayout)
                .For(x => x.Variants)
                .To(vm => vm.Variants);

            set.Apply();
        });
    }
}
