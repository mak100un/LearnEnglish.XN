using LearnEnglish.XN.Core.ViewModels;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace LearnEnglish.XN.Droid.Fragments;

[MvxFragmentPresentation(IsCacheableFragment = true, ActivityHostViewModelType = typeof(MainViewModel), FragmentContentId = Resource.Id.content_frame, AddToBackStack = true)]
public class ChatFragment : BaseFragment<ChatViewModel>
{
    protected override int ResourceId => Resource.Layout.chat_layout;
    protected override bool ShowHomeEnabled => true;

}
