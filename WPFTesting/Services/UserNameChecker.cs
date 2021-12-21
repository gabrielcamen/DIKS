using System.Linq;
using WPFTesting.Models.Interfaces;

namespace WPFTesting.Services
{
    public class UserNameChecker : IUserNameChecker
    {
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
