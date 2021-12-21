using System.Windows;
using Ninject;
using WpfApplication1.Modules;
using WpfApplication1.Services;
using WpfApplication1.ViewModels;
using WPFTesting;

namespace WpfApplication1
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
