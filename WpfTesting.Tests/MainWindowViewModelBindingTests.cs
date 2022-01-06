using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using WPFTesting.Models.Interfaces;
using WPFTesting.Services;
using WPFTesting.Services.Interfaces;
using WPFTesting.ViewModels;

namespace WpfTesting.Tests
{
    [TestClass]
    public class MainWindowViewModelBindingTests
    {
        private MoqMockingKernel _kernel = new MoqMockingKernel();
        
        [TestMethod]
        public void TestMessageSenderViewModel0()
        {
            //Arrange
            const string expectedCheckUserResult = "Good user!";
            
            var userNameChecker = new UserNameChecker();
            var windowService = new WindowService();
            var messageSenderService = new EmailSenderService();
            var messageSenderViewModel = new MessageSenderViewModel(messageSenderService);
            
            var viewModel = new MainWindowViewModel(userNameChecker, windowService, null, null);
            //Act
            viewModel.OnCheckUser.Execute(null);
            //Assert
            viewModel.CheckUserResultMessage.Should().Be(expectedCheckUserResult);
        }
        
        [TestMethod]
        public void TestMessageSenderViewModel1()
        {
            //Arrange
            const string expectedCheckUserResult = "Good user!";
            
            var userNameCheckerMock = new Mock<IUserNameChecker>();
            var windowServiceFake = new Mock<IWindowService>();
            var messageSenderViewModel = new Mock<IMessageSenderViewModelFactory>();
            userNameCheckerMock.Setup(m => m.IsUserNameSecure(It.IsAny<string>())).Returns(true);
            var viewModel = new MainWindowViewModel(userNameCheckerMock.Object, windowServiceFake.Object, messageSenderViewModel.Object, null);
            //Act
            viewModel.OnCheckUser.Execute(null);
            //Assert
            viewModel.CheckUserResultMessage.Should().Be(expectedCheckUserResult);
        }
        
        [TestMethod]
        public void TestMessageSenderViewModel()
        {
            //Arrange
            const string expectedCheckUserResult = "Good user!";
            var viewModel = _kernel.Get<MainWindowViewModel>();
            var messageSenderServiceMock = _kernel.GetMock<IUserNameChecker>();
            messageSenderServiceMock.Setup(m => m.IsUserNameSecure(It.IsAny<string>())).Returns(true);
            //Act
            viewModel.OnCheckUser.Execute(null);
            //Assert
            viewModel.CheckUserResultMessage.Should().Be(expectedCheckUserResult);
        }
    }
}
