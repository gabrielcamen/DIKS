using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Ninject;
using WPFTesting.Annotations;
using WPFTesting.Models;
using WPFTesting.Models.Interfaces;
using WPFTesting.Services;
using WPFTesting.Services.Interfaces;

namespace WPFTesting.ViewModels
{
    public interface IMessageSenderViewModelFactory
    {
        IMessageSenderViewModel Create();
    }
    
    public interface IMessageSenderViewModel
    {
        double EmailPercent { get; set; }
        Task SendEmail();
    }
    
    public class MessageSenderViewModel : IMessageSenderViewModel, INotifyPropertyChanged
    {
        private double _emailPercent = 0;
        private string _emailActionResult;
        private readonly IMessageSenderService _messageSenderService;
        public double EmailPercent
        {
            get => _emailPercent;
            set
            {
                _emailPercent = value;
                OnPropertyChanged(nameof(EmailPercent));
            }
        }
        public string EmailActionResult
        {
            get => _emailActionResult;
            set
            {
                _emailActionResult = value;
                OnPropertyChanged(nameof(EmailActionResult));
            }
        } 

        public MessageSenderViewModel(IMessageSenderService messageSenderService)
        {
            _messageSenderService = messageSenderService;
            LoadEvents();
        }

        private void LoadEvents()
        {
            _messageSenderService.MessagePercentageChanged += (sender, i) => EmailPercent = i;
        }

        public async Task SendEmail()
        {
            EmailActionResult = await _messageSenderService.SendMessage();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
