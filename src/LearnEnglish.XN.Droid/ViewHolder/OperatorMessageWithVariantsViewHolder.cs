using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LearnEnglish.XN.Core.ViewModels.Items;
using LearnEnglish.XN.Droid.Widgets;
using MvvmCross.Binding.BindingContext;

namespace LearnEnglish.XN.Droid.ViewHolder;

public class OperatorMessageWithVariantsViewHolder : BaseMessageViewHolder
{
    public OperatorMessageWithVariantsViewHolder(View itemView, IMvxBindingContext bindingContext)
        : base(itemView, bindingContext)
    {
        OperatorMessageWithVariantsTextView = itemView.FindViewById<TextView>(Resource.Id.operator_message_with_variants_textview);
        VariantsLayout = itemView.FindViewById<VariantsLayout>(Resource.Id.variantslayout);

        this.DelayBind(() =>
        {
            var set = this.CreateBindingSet<OperatorMessageWithVariantsViewHolder, MessageViewModel>();

            set.Bind(OperatorMessageWithVariantsTextView)
                .For(x => x.Text)
                .To(vm => vm.Text);

            set.Bind(VariantsLayout)
                .For(x => x.DataContext)
                .To(vm => vm);

            set.Bind(VariantsLayout)
                .For(x => x.Variants)
                .To(vm => vm.Variants);

            set.Apply();
        });
    }

    public TextView OperatorMessageWithVariantsTextView { get; }

    public VariantsLayout VariantsLayout { get; }
}
