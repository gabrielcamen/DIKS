using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WpfApplication1.Models.Interfaces;
using WpfApplication1.Services;
using WpfApplication1.ViewModels;

namespace WpfTesting.Tests
{
    [TestClass]
    public class MainWindowViewModelNoBindingTests
    {
        [TestMethod]
        public void TestMessageSenderViewModel1()
        {
            //Arrange
            const string expectedCheckUserResult = "Good user!";
            
            var viewModel = new MainWindowViewModel
            {
                User =
                {
                    Name = "dA1"
                }
            };
            //Act
            viewModel.OnCheckUser.Execute(null);
            //Assert
            viewModel.CheckUserResultMessage.Should().Be(expectedCheckUserResult);
        }
    }
}
