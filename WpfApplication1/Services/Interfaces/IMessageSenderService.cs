using System;
using System.Threading.Tasks;

namespace WpfApplication1.Services.Interfaces
{
    public interface IMessageSenderService
    {
        event EventHandler<int> MessagePercentageChanged;
        Task<string> SendMessage();
    }
}
