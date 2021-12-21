using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.MockingKernel.Moq;
using WPFTesting.Services;
using WPFTesting.Services.Interfaces;
using WPFTesting.ViewModels;

namespace WpfTesting.Tests
{
    [TestClass]
    public class MessageSenderViewModelTests
    {
        private MoqMockingKernel _kernel = new MoqMockingKernel();

        [TestMethod]
        public void TestMessageSenderViewModel()
        {
            //Arrange
            const string expectedEmailActionResult = "dd";
            var viewModel = _kernel.Get<MessageSenderViewModel>();
            var messageSenderServiceMock = _kernel.GetMock<IMessageSenderService>();
            messageSenderServiceMock.Setup(m => m.SendMessage()).Returns(Task.FromResult(expectedEmailActionResult));
            //Act
            viewModel.SendEmail();
            //Assert
            viewModel.EmailActionResult.Should().Be(expectedEmailActionResult);
        }
        
    }
}
