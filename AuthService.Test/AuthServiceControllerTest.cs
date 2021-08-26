using AuthMicroService.Interfaces;
using AuthMicroService.Models;
using Moq;
using NUnit.Framework;

namespace AuthMicroService.Test
{
    public class AuthServiceControllerTest
    {
        private readonly Mock authServiceMoq;
        private readonly Mock logger;

        public AuthServiceControllerTest()
        {
            authServiceMoq = new Mock<IAuthService>();
            logger = new Mock<Logger>
        }
        
        [Test]
        [Test(new User{EmailId = "", Password="1234"})]
        [Test(new User{EmailId = "test@test.com", Password=""})]
        public void Login_returns_BadRequest_When_Credentials_Empty(User userParam)
        {
            //Arrange
            

            //Act
            var controller = new AuthController();

            //Assert
        }
    }
}