using System;
using System.Threading.Tasks;
using WpfApplication1.Services.Interfaces;

namespace WpfApplication1.Services
{
    public class EmailSenderService : IMessageSenderService
    {
        public event EventHandler<int> MessagePercentageChanged;
        private void OnEmailPercentageChanged(int percentage)
        {
            MessagePercentageChanged?.Invoke(this, percentage);
        }
        
        public async Task<string> SendMessage()
        {
            try
            {
                for (int percent = 0; percent < 100; percent++)
                {
                    OnEmailPercentageChanged(percent);
                    await Task.Delay(5);
                }

                return "Email sent";
            }
            catch   
            {
                return "Something went wrong, email not sent";
            }
            
        }
    }
}
