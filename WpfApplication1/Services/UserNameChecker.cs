using System.Linq;
using WpfApplication1.Models.Interfaces;

namespace WpfApplication1.Services
{
    public class UserNameChecker : IUserNameChecker
    {
        //private Service otherService = new Services()
        public bool IsUserNameSecure(string userName)
        {
            if (userName == null)
                return false;

            if (!userName.Any(char.IsUpper))
                return false;

            if (!userName.Any(char.IsNumber))
                return false;

            return true;
        }
    }
}
