namespace WPFTesting.Models.Interfaces
{
    public interface IUserNameChecker
    {
        bool IsUserNameSecure(string user);
    }
}
