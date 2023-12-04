using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Pharmacy.Controllers;
using Pharmacy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Tests
{
    internal class CartControllerTests
    {
        private readonly Mock<ICartRepository> _cartRepo;

        public CartControllerTests()
        {
            _cartRepo = new Mock<ICartRepository>();
        }

        private CartController CartController => new CartController(
           _cartRepo.Object
        );

        [Test]
        public async Task AddItem_ReturnsOk()
        {
            // Arrange
            _cartRepo.Setup(x => x.AddItem(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await CartController.AddProduct(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task RemoveItem_ReturnsOk()
        {
            // Arrange
            _cartRepo.Setup(x => x.RemoveItem(It.IsAny<string>(), It.IsAny<int>()));
            var controller = CartController;

            // Act
            var result = await controller.RemoveItem(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserCart_ReturnsOk()
        {
            // Arrange
            _cartRepo.Setup(x => x.GetUserCart(It.IsAny<string>())).ReturnsAsync(new Entities.Cart());
            var controller = CartController;

            // Act
            var result = await controller.GetUserCart(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetTotalItemInCart_ReturnsOk()
        {
            // Arrange
            _cartRepo.Setup(x => x.GetCartItemCount(It.IsAny<string>())).ReturnsAsync(It.IsAny<int>());
            var controller = CartController;

            // Act
            var result = await controller.GetTotalItemInCart(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Checkout_ReturnsOk()
        {
            // Arrange
            _cartRepo.Setup(x => x.DoCheckout(It.IsAny<string>())).ReturnsAsync(true);
            var controller = CartController;

            // Act
            var result = await controller.Checkout("e40fb9d4-2b92-46a0-a58f-fc2324889b63");

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}
