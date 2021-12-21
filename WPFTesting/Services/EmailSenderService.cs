using System;
using System.Threading.Tasks;
using WPFTesting.Services.Interfaces;

namespace WPFTesting.Services
{
    public class EmailSenderService : IMessageSenderService
    {
        public event EventHandler<int> MessagePercentageChanged;
        private void OnEmailPercentageChanged(int percentage)
        {
            MessagePercentageChanged?.Invoke(this, percentage);
        }

        public int Percent { get; set; } = 0;
        
        public async Task<string> SendMessage()
        {
            try
            {
                do
                {
                    OnEmailPercentageChanged(Percent);
                    await Task.Delay(5);
                    Percent++;
                } while (Percent < 100);

                return "Email sent";
            }
            catch   
            {
                return "Something went wrong, email not sent";
            }
            
        }
    }
}
