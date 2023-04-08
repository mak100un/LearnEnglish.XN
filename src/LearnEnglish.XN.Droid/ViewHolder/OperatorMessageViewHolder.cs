using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LearnEnglish.XN.Core.ViewModels.Items;
using MvvmCross.Binding.BindingContext;

namespace LearnEnglish.XN.Droid.ViewHolder;

public class OperatorMessageViewHolder : BaseMessageViewHolder
{
    public OperatorMessageViewHolder(View itemView, IMvxBindingContext bindingContext)
        : base(itemView, bindingContext)
    {
        OperatorMessageTextView = itemView.FindViewById<TextView>(Resource.Id.operator_message_textview);

        this.DelayBind(() =>
        {
            var set = this.CreateBindingSet<OperatorMessageViewHolder, MessageViewModel>();

            set.Bind(OperatorMessageTextView)
               .For(x => x.Text)
               .To(vm => vm.Text);

            set.Apply();
        });
    }

    public TextView OperatorMessageTextView { get; }
}
