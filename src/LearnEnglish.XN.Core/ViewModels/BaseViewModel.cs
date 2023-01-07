using System;
using System.Threading.Tasks;
using LearnEnglish.XN.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PropertyChanged;

namespace LearnEnglish.XN.Core.ViewModels;

public abstract class BaseViewModel : MvxViewModel
{
    protected BaseViewModel(ILogger logger)
    {
        Logger = logger;
    }

    public bool IsLoading { get; set; }

    [MvxInject]
    [DoNotNotify]
    public IMvxNavigationService NavigationService { get; set; }

    [MvxInject]
    [DoNotNotify]
    public IDialogService DialogService { get; set; }

    public ILogger Logger { get; }

    protected MvxNotifyTask CreateMvxNotifyTask(Func<Task> task, Action<Exception> onException = null) =>
        MvxNotifyTask.Create(task, ex =>
        {
            Logger.LogError(ex, ex.Message);
            onException?.Invoke(ex);
        });
}
