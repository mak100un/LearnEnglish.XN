using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;

namespace LearnEnglish.XN.Droid.ViewHolder;

public class OperatorMessageViewHolder : BaseMessageViewHolder
{
    public OperatorMessageViewHolder(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public OperatorMessageViewHolder(View itemView, IMvxBindingContext bindingContext)
        : base(itemView, bindingContext)
    {
        OperatorMessageTextView = itemView.FindViewById<TextView>(Resource.Id.operator_message_textview);
    }

    public TextView OperatorMessageTextView { get; }
}
