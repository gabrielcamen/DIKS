using Ninject.Extensions.Factory;
using Ninject.Modules;
using WpfApplication1.Models.Interfaces;
using WpfApplication1.Services;
using WpfApplication1.Services.Interfaces;
using WpfApplication1.ViewModels;

namespace WpfApplication1.Modules
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
            Bind<IMessageSenderService>().To<EmailSenderService>().WhenInjectedExactlyInto<MessageSenderViewModel>().InSingletonScope();
        }
    }
}
