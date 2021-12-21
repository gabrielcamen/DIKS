using System.Windows;
using Ninject;
using WPFTesting.Modules;
using WPFTesting.Services;
using WPFTesting.ViewModels;

namespace WPFTesting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private IKernel _container;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureContainer();
            ComposeView();

            base.OnStartup(e);
        }

        private void ComposeView()
        {
            _container.Get<IWindowService>().ShowWindow<MainWindow>(_container.Get<MainWindowViewModel>());
        }

        private void ConfigureContainer()
        {
            _container = new StandardKernel(new BindingModule());
        }
    }
}
