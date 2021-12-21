using System;
using System.Threading.Tasks;

namespace WPFTesting.Services.Interfaces
{
    public interface IMessageSenderService
    {
        event EventHandler<int> MessagePercentageChanged;
        Task<string> SendMessage();
    }
}
