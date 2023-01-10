using System;
using System.IO;
using System.Net.Http;
using LearnEnglish.XN.Core;
using LearnEnglish.XN.Core.Definitions.Constants;
using LearnEnglish.XN.Core.Repositories;
using LearnEnglish.XN.Core.Repositories.Interfaces;
using LearnEnglish.XN.Core.Services;
using LearnEnglish.XN.Core.Services.Interfaces;
using LearnEnglish.XN.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using SQLite;

namespace LearnEnglish.XN
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton(() => new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbConstants.DB_NAME)));
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMessagesRepository, MessagesRepository>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<HttpClient, HttpClient>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton(Mapper.Build);

            RegisterAppStart<LaunchViewModel>();
        }
    }
}
