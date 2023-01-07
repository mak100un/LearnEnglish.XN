using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace LearnEnglish.XN.Core.Services.Interfaces;

public interface INavigationService : IMvxNavigationService
{
    IList<IMvxViewModel> NavigationStack { get; }

    Task<bool> NavigateBackAsync(CancellationToken token = default);

    bool CanNavigateBack();
}
