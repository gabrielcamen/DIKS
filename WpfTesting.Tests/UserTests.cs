using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WPFTesting.Models;
using WPFTesting.Models.Interfaces;

namespace WpfTesting.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod] 
        public void CreatingEmptyUserShouldDefaultEmptyName()
        {
            //Arrange/Assert
            var user = new User();
            //Act
            user.Name.Should().Be(null);
        }

        [TestMethod]
        public void CreateUserWithNameShouldCreateProperUser()
        {
            //Arrange
            const string name = "Andrei";
            //Act
            var user = new User(name);
            //Assert
            user.Name.Should().Be(name);
        }

        [TestMethod]
        public void Test()
        {
            //Arrange
            const string userName = "Doru";
            var userNameCheckerMock = new Mock<IUserNameChecker>() {CallBase = true};
            // userNameCheckerMock.Setup(checker => checker.CheckUser(It.IsAny<User>())).Returns(true);
            
            // var user = new User(userName);
            //Act
            // var result = userNameCheckerMock.Object.IsUserNameSecure(user);
            //Assert
            // result.Should().Be(true);

        }


    }
}
