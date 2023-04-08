using Android.Views;
using Android.Widget;
using LearnEnglish.XN.Core.ViewModels.Items;
using MvvmCross.Binding.BindingContext;

namespace LearnEnglish.XN.Droid.ViewHolder;

public class MyMessageViewHolder : BaseMessageViewHolder
{
    public MyMessageViewHolder(View itemView, IMvxBindingContext bindingContext)
        : base(itemView, bindingContext)
    {
        MyMessageTextView = itemView.FindViewById<TextView>(Resource.Id.my_message_textview);

        this.DelayBind(() =>
        {
            var set = this.CreateBindingSet<MyMessageViewHolder, MessageViewModel>();

            set.Bind(MyMessageTextView)
             .For(x => x.Text)
             .To(vm => vm.Text);

            set.Apply();
        });
    }

    public TextView MyMessageTextView { get; }
}
