using System.Windows;

namespace WpfApplication1.Services
{
    public interface IWindowService
    {
        void ShowWindow<T>(object viewModel) where T: Window, new();
    }
    
    public class WindowService : IWindowService
    {
        public void ShowWindow<T>(object viewModel) where T: Window, new()
        {
            var win = new T
            {
                DataContext = viewModel
            };
            win.Show();
        }
        
    }
}
