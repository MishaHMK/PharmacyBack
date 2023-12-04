using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Pharmacy.Controllers;
using Pharmacy.Entities;
using Pharmacy.Services;
using User = Pharmacy.Entities.User;

namespace Pharmacy.Tests
{
    internal class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accService;
        private readonly Mock<IJWTService> _jWTManager;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<SignInManager<User>> _signInManager;
        private Mock<IHttpContextAccessor> _contextAccessor;
        private Mock<IUserClaimsPrincipalFactory<User>> _principalFactory;

        public AccountControllerTests()
        {
            _accService = new Mock<IAccountService>();
            _jWTManager = new Mock<IJWTService>();
            _userManager = new Mock<UserManager<User>>(MockBehavior.Strict, Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _userManager.SetupSet(m => m.Logger = null);
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _principalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _signInManager = new Mock<SignInManager<User>>(_userManager.Object, _contextAccessor.Object, _principalFactory.Object, null, null, null, null);
        }

        private AccountController AccountController => new AccountController(
           _userManager.Object,
           _signInManager.Object,
           _accService.Object,
           _jWTManager.Object
        );

        [Test]
        public async Task CheckRoles_ReturnsOk()
        {
            // Arrange
            _accService.Setup(x => x.GetRoles())
                           .ReturnsAsync(new List<string>());

            // Act
            var result = await AccountController.CheckRoles();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserById_ReturnsOk()
        {
            // Arrange
            _accService.Setup(x => x.GetUserById(It.IsAny<string>(), It.IsAny<string>()))
                          .ReturnsAsync(user);

            // Act
            var result = await AccountController.GetUserById(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task Register_ReturnsNoContent()
        {
            // Arrange
            var registerDto = new Register()
            {
                Email = ""
            };
            var userDto = new User()
            {
                Id = "",
                Email = registerDto.Email
            };

            _userManager.Setup(x => x.CreateAsync(It.Is<User>(v => v.Email == userDto.Email), It.Is<string>(v => v == registerDto.Password)))
                           .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRoleAsync(It.Is<User>(v => v.Email == userDto.Email), It.Is<string>(v => v == registerDto.RoleName)))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = AccountController.Register(registerDto).Result;

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        private User user => new User()
        {
            Name = "Mykhailo",
            FatherName = "Igorovych",
            Surname = "Humeniuk"
        };

        private Register register => new Register()
        {
            Name = "Mykhailo",
            Fathername = "Igorovych",
            Surname = "Humeniuk",
            Email = "xan@gmail.com",
            Password = "12345678Ar*",
            ConfirmPassword = "12345678Ar*",
            RoleName = "Admin"
        };
    }
}