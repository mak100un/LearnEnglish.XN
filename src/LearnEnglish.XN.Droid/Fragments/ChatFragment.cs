using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using LearnEnglish.XN.Core.ViewModels;
using LearnEnglish.XN.Droid.Adapters;
using LearnEnglish.XN.Droid.Listeners;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace LearnEnglish.XN.Droid.Fragments;

[MvxFragmentPresentation(IsCacheableFragment = true, ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.content_frame, AddToBackStack = true)]
public class ChatFragment : BaseFragment<ChatViewModel>
{
    protected override int ResourceId => Resource.Layout.chat_layout;
    protected override bool ShowHomeEnabled => true;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var view = base.OnCreateView(inflater, container, savedInstanceState);

        var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.messages_recyclerview);
        var layoutManager = recyclerView.GetLayoutManager() as LinearLayoutManager;
        layoutManager.StackFromEnd = true;
        layoutManager.ReverseLayout = false;
        var adapter = new MessagesAdapter(BindingContext as IMvxAndroidBindingContext, layoutManager, () => recyclerView?.SmoothScrollToPosition(ViewModel.Messages.Count - 1));
        recyclerView.SetAdapter(adapter);

        var scrollListener = new RecyclerPaginationListener(layoutManager);
        recyclerView.AddOnScrollListener(scrollListener);

        var set = CreateBindingSet();

        set.Bind(scrollListener)
            .For(x => x.LoadMoreCommand)
            .To(vm => vm.LoadMoreCommand);

        set.Bind(scrollListener)
            .For(x => x.LoadingOffset)
            .To(vm => vm.LoadingOffset);

        set.Bind(adapter)
            .For(x => x.Messages)
            .To(vm => vm.Messages);

        set.Apply();

        return view;
    }

}
