using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using LearnEnglish.XN.Core.ViewModels.Items;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace LearnEnglish.XN.iOS.Cells;

public class MyMessageCell : BaseMessageCell
{
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
        var labelView = new UIView
        {
            BackgroundColor = UIColor.FromRGB(242, 243, 247),
        };
        labelView.Add(label);
        labelView.Layer.CornerRadius = 16;
        labelView.Layer.MasksToBounds = true;

        Add(labelView);

        NSLayoutConstraint.ActivateConstraints(new []
        {
            // label
            label.BottomAnchor.ConstraintEqualTo(labelView.BottomAnchor, -10),
            label.TrailingAnchor.ConstraintEqualTo(labelView.TrailingAnchor, -16),
            label.LeadingAnchor.ConstraintEqualTo(labelView.LeadingAnchor, 16),
            label.TopAnchor.ConstraintEqualTo(labelView.TopAnchor, 10),

            // labelView
            labelView.LeadingAnchor.ConstraintGreaterThanOrEqualTo(LeadingAnchor, 100),
            labelView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -16),
            labelView.TopAnchor.ConstraintEqualTo(TopAnchor, 8),
            labelView.BottomAnchor.ConstraintEqualTo(BottomAnchor, -8),
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
}
