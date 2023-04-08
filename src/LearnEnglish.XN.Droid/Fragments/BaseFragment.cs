using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace LearnEnglish.XN.Droid.Fragments;

public abstract class BaseFragment<TViewModel> : MvxFragment<TViewModel>
    where TViewModel : MvxViewModel
{
    protected abstract int ResourceId { get; }

    protected abstract bool ShowHomeEnabled { get; }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);
        return this.BindingInflate(ResourceId, container, false);
    }

    public override void OnStart()
    {
        base.OnStart();

        if (Activity?.ActionBar is not { } actionBar)
        {
            return;
        }

        actionBar.SetDisplayHomeAsUpEnabled(ShowHomeEnabled);
        actionBar.SetHomeButtonEnabled(ShowHomeEnabled);
    }
}
