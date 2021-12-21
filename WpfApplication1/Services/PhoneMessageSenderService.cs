using System;
using System.Threading.Tasks;
using WpfApplication1.Services.Interfaces;

namespace WpfApplication1.Services
{
    public class PhoneMessageSenderService : IMessageSenderService
    {
        public event EventHandler<int> MessagePercentageChanged;
        public async Task<string> SendMessage()
        {
            try
            {
                for (int percent = 0; percent < 100; percent++)
                {
                    OnEmailPercentageChanged(percent);
                    await Task.Delay(5);
                }

                return "Phone message sent";
            }
            catch   
            {
                return "Something went wrong, message not sent";
            }
        }

        private void OnEmailPercentageChanged(int percent)
        {
            MessagePercentageChanged?.Invoke(this, percent);
        }
    }
}
