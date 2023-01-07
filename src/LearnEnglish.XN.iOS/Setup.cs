using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Ios.Core;
using LearnEnglish.XN.Core.Services.Interfaces;
using LearnEnglish.XN.iOS.Services;
using MvvmCross.IoC;
using Serilog;
using Serilog.Extensions.Logging;

namespace LearnEnglish.XN.iOS
{
    public class Setup : MvxIosSetup<App>
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
                .WriteTo.NSLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}
