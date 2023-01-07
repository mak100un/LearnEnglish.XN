using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Android.Core;
using LearnEnglish.XN.Core.Services.Interfaces;
using LearnEnglish.XN.Droid.Services;
using MvvmCross.IoC;
using Serilog;
using Serilog.Extensions.Logging;

namespace LearnEnglish.XN.Droid
{
    public class Setup : MvxAndroidSetup<App>
    {
        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);
            iocProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
        }

        protected override IMvxIocOptions CreateIocOptions() => new MvxIocOptions
        {
            PropertyInjectorOptions = MvxPropertyInjectorOptions.MvxInject
        };

        protected override ILoggerProvider CreateLogProvider() => new SerilogLoggerProvider();

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.AndroidLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}
