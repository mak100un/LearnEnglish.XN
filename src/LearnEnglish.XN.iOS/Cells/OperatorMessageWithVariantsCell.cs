using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using LearnEnglish.XN.Core.ViewModels.Items;
using LearnEnglish.XN.iOS.Extensions;
using LearnEnglish.XN.iOS.Views;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class OperatorMessageWithVariantsCell : BaseMessageCell
{
    private UIView _labelView;

    [Export("initWithFrame:")]
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
            TranslatesAutoresizingMaskIntoConstraints = false,
            Lines = 0,
            LineBreakMode = UILineBreakMode.WordWrap
        };

        _labelView = new UIView
        {
            BackgroundColor = UIColor.FromRGB(145, 215, 255),
        };
        _labelView.Add(label);

        if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
        {
            _labelView.Layer.CornerRadius = 16;
            _labelView.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner | CACornerMask.MaxXMaxYCorner;
        }
        _labelView.Layer.MasksToBounds = true;

        Add(_labelView);

        var variantsLayout = new VariantsLayout
        {
            MaxWidth = UIScreen.MainScreen.Bounds.Width - 100f - 16f,
        };

        Add(variantsLayout);

        NSLayoutConstraint.ActivateConstraints(new []
        {
            // label
            label.BottomAnchor.ConstraintEqualTo(_labelView.BottomAnchor, -10),
            label.TrailingAnchor.ConstraintEqualTo(_labelView.TrailingAnchor, -16),
            label.LeadingAnchor.ConstraintEqualTo(_labelView.LeadingAnchor, 16),
            label.TopAnchor.ConstraintEqualTo(_labelView.TopAnchor, 10),

            // labelView
            _labelView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 16),
            _labelView.TrailingAnchor.ConstraintLessThanOrEqualTo(TrailingAnchor, -100),
            _labelView.TopAnchor.ConstraintEqualTo(TopAnchor, 8),

            // variantsLayout
            variantsLayout.TopAnchor.ConstraintEqualTo(_labelView.BottomAnchor, 8),
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

        this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
        {
            return;
        }

        _labelView.AddCornerRadius(UIRectCorner.TopLeft | UIRectCorner.TopRight | UIRectCorner.BottomRight, 16);
    }
}
