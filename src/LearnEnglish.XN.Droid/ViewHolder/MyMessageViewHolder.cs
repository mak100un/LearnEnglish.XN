using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;

namespace LearnEnglish.XN.Droid.ViewHolder;

public class MyMessageViewHolder : BaseRecyclerViewHolder
{
    public MyMessageViewHolder(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public MyMessageViewHolder(View itemView, IMvxBindingContext bindingContext)
        : base(itemView, bindingContext)
    {
        MyMessageTextView = itemView.FindViewById<TextView>(Resource.Id.my_message_textview);
    }

    public TextView MyMessageTextView { get; }
}
