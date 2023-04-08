using LearnEnglish.XN.Core.Definitions.Constants;
using LearnEnglish.XN.Core.Services.Interfaces;
using Xamarin.Essentials;

namespace LearnEnglish.XN.Core.Services;

public class PropertyStorageService : IPropertyStorageService
{
    public string UserName
    {
        get => Preferences.Get(nameof(UserName), AppConstants.DEFAULT_USER_NAME);
        set => Preferences.Set(nameof(UserName), value ?? AppConstants.DEFAULT_USER_NAME);
    }
}
