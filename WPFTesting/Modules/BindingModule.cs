using Ninject.Extensions.Factory;
using Ninject.Modules;
using WPFTesting.Models.Interfaces;
using WPFTesting.Services;
using WPFTesting.Services.Interfaces;
using WPFTesting.ViewModels;

namespace WPFTesting.Modules
{
    public class BindingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserNameChecker>().To<UserNameChecker>();

            Bind<IWindowService>().To<WindowService>().InSingletonScope();

            Bind<MainWindowViewModel>().ToSelf().InTransientScope();

            Bind<IMessageSenderViewModel>().To<MessageSenderViewModel>().InTransientScope().Named("name1");
            
            Bind<IMessageSenderViewModelFactory>().ToFactory();

            Bind<IMessageSenderService>().To<PhoneMessageSenderService>().WhenAnyAncestorNamed("name").InSingletonScope();
            Bind<IMessageSenderService>().To<EmailSenderService>().WhenInjectedExactlyInto<MessageSenderViewModel>().InTransientScope();
        }
    }
}
