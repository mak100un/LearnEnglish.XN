using System.Collections.Generic;
using System.Windows.Input;
using LearnEnglish.XN.Core.Definitions.Constants;
using LearnEnglish.XN.Core.Definitions.Enums;
using LearnEnglish.XN.Core.Services.Interfaces;
using LearnEnglish.XN.Core.ViewModels.Items;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using PropertyChanged;

namespace LearnEnglish.XN.Core.ViewModels;

public class LaunchViewModel : BaseViewModel
{
    private readonly IPropertyStorageService _propertyStorageService;

    [DoNotNotify]
    public ICommand ActionCommand { get; set; }

    public LaunchViewModel(IPropertyStorageService propertyStorageService, ILogger<LaunchViewModel> logger)
        : base(logger)
    {
        _propertyStorageService = propertyStorageService;

        ActionCommand = new MvxAsyncCommand<LaunchActionTypes>(async actionType =>
        {
            switch (actionType)
            {
                case LaunchActionTypes.UserName:
                    UserName = await DialogService.DisplayInputAsync(
                                   "Задайте имя",
                                   "Какое имя вы хотели бы использовать в приложении?",
                                   UserName,
                                   "Подтвердить",
                                   "Отмена") ??
                               AppConstants.DEFAULT_USER_NAME;
                    break;
                case LaunchActionTypes.Chat:
                    await NavigationService.Navigate<ChatViewModel>();
                    break;
            }
        });
    }

    public string UserName
    {
        get => _propertyStorageService.UserName;
        set
        {
            if (_propertyStorageService.UserName == value
                || string.IsNullOrWhiteSpace(_propertyStorageService.UserName))
            {
                return;
            }

            _propertyStorageService.UserName = value;
            RaisePropertyChanged(() => UserName);
        }
    }
}
