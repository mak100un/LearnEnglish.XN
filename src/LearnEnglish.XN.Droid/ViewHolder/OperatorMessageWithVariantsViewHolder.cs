using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LearnEnglish.XN.Droid.Widgets;
using MvvmCross.Binding.BindingContext;

namespace LearnEnglish.XN.Droid.ViewHolder;

public class OperatorMessageWithVariantsViewHolder : BaseMessageViewHolder
{
    public OperatorMessageWithVariantsViewHolder(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public OperatorMessageWithVariantsViewHolder(View itemView, IMvxBindingContext bindingContext)
        : base(itemView, bindingContext)
    {

        OperatorMessageWithVariantsTextView = itemView.FindViewById<TextView>(Resource.Id.operator_message_with_variants_textview);
        VariantsLayout = itemView.FindViewById<VariantsLayout>(Resource.Id.variantslayout);
    }

    public TextView OperatorMessageWithVariantsTextView { get; }

    public VariantsLayout VariantsLayout { get; }
}
