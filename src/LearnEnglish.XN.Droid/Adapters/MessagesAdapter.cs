using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using LearnEnglish.XN.Core.Definitions.Enums;
using LearnEnglish.XN.Core.Definitions.Extensions;
using LearnEnglish.XN.Core.ViewModels.Items;
using LearnEnglish.XN.Droid.ViewHolder;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PropertyChanged;
using Swordfish.NET.Collections;

namespace LearnEnglish.XN.Droid.Adapters;

public class MessagesAdapter :  RecyclerView.Adapter, INotifyPropertyChanged
{
    private readonly Action _onItemAdded;
    private readonly LinearLayoutManager _layoutManager;

    public MessagesAdapter(IMvxAndroidBindingContext context, LinearLayoutManager layoutManager, Action onItemAdded)
    {
        BindingContext = context;
        _layoutManager = layoutManager;
        _onItemAdded = onItemAdded;
    }

    public IMvxAndroidBindingContext BindingContext { get; }

    public override int ItemCount => Messages?.Count ?? 0;

    [OnChangedMethod(nameof(OnMessagesChanged))]
    public ConcurrentObservableCollection<MessageViewModel> Messages { get; set; }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        var message = Messages?.ElementAtOrDefault(position);

        if (message == null
            || message.MessageType == MessageTypes.Loading
            || holder is not BaseMessageViewHolder viewHolder)
        {
            return;
        }

        viewHolder.DataContext = message;

        var set = viewHolder.CreateBindingSet<BaseMessageViewHolder, MessageViewModel>();
        switch (holder)
        {
            case OperatorMessageViewHolder operatorMessageViewHolder:

                set.Bind(operatorMessageViewHolder.OperatorMessageTextView)
                    .For(x => x.Text)
                    .To(vm => vm.Text);

                break;
            case OperatorMessageWithVariantsViewHolder operatorMessageWithVariantsViewHolder:

                set.Bind(operatorMessageWithVariantsViewHolder.OperatorMessageWithVariantsTextView)
                    .For(x => x.Text)
                    .To(vm => vm.Text);

                set.Bind(operatorMessageWithVariantsViewHolder.VariantsLayout)
                    .For(x => x.DataContext)
                    .To(vm => vm);

                set.Bind(operatorMessageWithVariantsViewHolder.VariantsLayout)
                    .For(x => x.Variants)
                    .To(vm => vm.Variants);

                break;
            case MyMessageViewHolder myMessageViewHolder:

                set.Bind(myMessageViewHolder.MyMessageTextView)
                    .For(x => x.Text)
                    .To(vm => vm.Text);

                break;
        }

        set.Apply();
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        var bindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
        return (MessageTypes)viewType switch
        {
            MessageTypes.Operator => new OperatorMessageViewHolder(bindingContext.BindingInflate(Resource.Layout.operator_message_item, parent, false), bindingContext),
            MessageTypes.OperatorWithVariants => new OperatorMessageWithVariantsViewHolder(bindingContext.BindingInflate(Resource.Layout.operator_message_item_with_variants, parent, false), bindingContext),
            MessageTypes.Loading => new LoadingViewHolder(bindingContext.BindingInflate(Resource.Layout.loader_item, parent, false)),
            _ => new MyMessageViewHolder(bindingContext.BindingInflate(Resource.Layout.my_message_item, parent, false), bindingContext),
        };
    }

    public override int GetItemViewType(int position) => (int)(Messages?.ElementAtOrDefault(position)?.MessageType ?? MessageTypes.Mine) ;

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnMessagesChanged(object oldValue, object newValue)
    {
        (oldValue as ConcurrentObservableCollection<MessageViewModel>)?.Then(value => value.CollectionChanged -= OnCollectionChanged);
        (newValue as ConcurrentObservableCollection<MessageViewModel>)?.Then(value => value.CollectionChanged += OnCollectionChanged);
        NotifyDataSetChanged();
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems?.Count == 1:
                {
                    var shouldScroll = e.NewStartingIndex != 0
                        && _layoutManager?.FindLastVisibleItemPosition() >= ItemCount - 3;
                    NotifyItemInserted(e.NewStartingIndex);
                    if (shouldScroll)
                    {
                        _onItemAdded?.Invoke();
                    }

                    break;
                }
            case NotifyCollectionChangedAction.Add:
                {
                    var shouldScroll = _layoutManager?.FindLastVisibleItemPosition() >= ItemCount - 3;
                    NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems!.Count);
                    if (shouldScroll)
                    {
                        _onItemAdded?.Invoke();
                    }
                    break;
                }
            case NotifyCollectionChangedAction.Remove when e.OldItems?.Count == 1:
                NotifyItemRemoved(e.OldStartingIndex);
                break;
            case NotifyCollectionChangedAction.Remove:
                NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems!.Count);
                break;
            default:
                NotifyDataSetChanged();
                break;
        }
    }

    protected override void Dispose(bool disposing)
    {
        Messages?.Then(value => value.CollectionChanged -= OnCollectionChanged);
        base.Dispose(disposing);
    }
}
