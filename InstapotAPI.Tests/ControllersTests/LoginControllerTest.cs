using AutoMapper;
using InstapotAPI.Controllers;
using InstapotAPI.Dtos.Profile;
using InstapotAPI.Entity;
using InstapotAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InstapotAPI.Tests.Controllers
{
    [TestClass]
    public class LoginControllerTest
    {

        private readonly Mock<IProfileRepository> _profileReposetory;

        private readonly Mock<ILogger<LoginController>> _logger;

        private readonly Mock<IMapper> _mapper;

        private LoginController _loginController;

        public LoginControllerTest()
        {
            _profileReposetory = new Mock<IProfileRepository>();
            _logger = new Mock<ILogger<LoginController>>();
            _mapper = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task If_Get_Is_Callde_Return_A_OkResult()
        {
            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Get();
            var actionResultType = (OkResult)actionResult;


            Assert.IsInstanceOfType(actionResultType, typeof(OkResult));
        }

        [TestMethod]
        public async Task If_Create_Is_Given_A_Valid_Profile_Return_Id_Username_Password_And_Email_Of_The_Associated_Profile()
        {
            var profile = new Entity.Profile() { Username = "test", Password = "testpass", Email = "testmail" };

            _profileReposetory.Setup(p => p.Create(profile)).ReturnsAsync(profile);

            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Create(profile);
            var actionResultType = actionResult.Result;
            var result = (Entity.Profile)((CreatedAtActionResult)actionResultType).Value;


            Assert.AreEqual(profile.Id, result.Id);
            Assert.AreEqual(profile.Username, result.Username);
            Assert.AreEqual(profile.Password, result.Password);
            Assert.AreEqual(profile.Email, result.Email);
        }

        [TestMethod]
        public async Task If_Create_Is_Given_A_Valid_Profile_Return_Created_AtActionResult()
        {
            var profile = new Entity.Profile { Username = "test", Password = "testpass", Email = "testmail" };

            _profileReposetory.Setup(p => p.Create(profile)).ReturnsAsync(profile);

            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Create(profile);
            var actionResultType = actionResult.Result;


            Assert.IsInstanceOfType(actionResultType, typeof(CreatedAtActionResult));

        }

        [TestMethod]
        public async Task If_SetLoginStatusToTrue_Is_Given_A_Existing_Id_Return_A_OkObjectResult_That_Contains_The_Expected_Bool()
        {
            var expected = true;

            var existingId = It.IsAny<int>();

            _profileReposetory.Setup(p => p.SetLoginStatusToTrue(existingId)).ReturnsAsync(expected);

            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Login(existingId);
            var actionResultType = (OkObjectResult)actionResult;
            var result = (bool)actionResultType.Value;


            Assert.IsInstanceOfType(actionResultType, typeof(OkObjectResult));
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task If_SetLoginStatusToTrue_Is_Given_A_Nonexisting_Id_Return_A_NotFoundResult()
        {

            var nonExistingId = It.IsAny<int>();

            _profileReposetory.Setup(p => p.SetLoginStatusToTrue(nonExistingId)).ReturnsAsync((bool?)null);

            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Login(nonExistingId);
            var actionResultType = (NotFoundObjectResult)actionResult;


            Assert.IsInstanceOfType(actionResultType, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task If_SetLoginStatusToFalse_Is_Given_A_Existing_Id_Return_A_OkObjectResult_That_Contains_The_Expected_Bool()
        {
            var expected = false;

            var existingId = It.IsAny<int>();

            _profileReposetory.Setup(p => p.SetLoginStatusToFalse(existingId)).ReturnsAsync(expected);

            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Logout(existingId);
            var actionResultType = (OkObjectResult)actionResult;
            var result = (bool)actionResultType.Value;


            Assert.IsInstanceOfType(actionResultType, typeof(OkObjectResult));
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task If_SetLoginStatusToFalse_Is_Given_A_Nonexisting_Id_Return_A_NotFoundResult()
        {
            var nonExistingId = It.IsAny<int>();

            _profileReposetory.Setup(p => p.SetLoginStatusToFalse(nonExistingId)).ReturnsAsync((bool?)null);

            _loginController = new LoginController(_profileReposetory.Object, _logger.Object);


            var actionResult = await _loginController.Login(nonExistingId);
            var actionResultType = (NotFoundObjectResult)actionResult;


            Assert.IsInstanceOfType(actionResultType, typeof(NotFoundObjectResult));
        }
    }
}
