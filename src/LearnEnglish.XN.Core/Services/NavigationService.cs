using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LearnEnglish.XN.Core.Definitions.Extensions;
using LearnEnglish.XN.Core.Services.Interfaces;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace LearnEnglish.XN.Core.Services;

public class NavigationService : MvxNavigationService, INavigationService
{
    public NavigationService(IMvxViewModelLoader viewModelLoader, IMvxViewDispatcher viewDispatcher, IMvxIoCProvider iocProvider)
        : base(viewModelLoader, viewDispatcher, iocProvider)
    {
    }

    public IList<IMvxViewModel> NavigationStack { get; } = new List<IMvxViewModel>();

    public Task<bool> NavigateBackAsync(CancellationToken token = default)
    {
        if (!CanNavigateBack())
        {
            return Task.FromResult(false);
        }

        return Close(NavigationStack.Last(), token);
    }

    public bool CanNavigateBack() => NavigationStack.Count > 1;

    protected override void OnDidNavigate(object sender, IMvxNavigateEventArgs e)
    {
        base.OnDidNavigate(sender, e);
        NavigationStack.Add(e.ViewModel);
    }

    protected override void OnDidClose(object sender, IMvxNavigateEventArgs e)
    {
        base.OnDidClose(sender, e);
        NavigationStack.TryRemove(e.ViewModel);
    }
}
