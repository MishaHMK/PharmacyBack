using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Pharmacy.Controllers;
using Pharmacy.Entities;
using Pharmacy.Helpers;
using Pharmacy.Repositories;
using Pharmacy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Tests
{
    internal class ProductControllerTests
    {
        private readonly Mock<IProductsRepository> _productRepo;

        public ProductControllerTests()
        {
            _productRepo = new Mock<IProductsRepository>();
        }

        private ProductsController ProductsController => new ProductsController(
           _productRepo.Object
        );

        [Test]
        public async Task GetAllProducts_ReturnsOk()
        {
            // Arrange
            _productRepo.Setup(x => x.GetProductList())
                           .ReturnsAsync(new List<Product>());

            // Act
            var result = await ProductsController.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetProductById_ReturnsOk()
        {
            // Arrange
            _productRepo.Setup(x => x.GetProductById(It.IsAny<int>()))
                           .ReturnsAsync(new Product());

            // Act
            var result = await ProductsController.GetProductById(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditProductById_ReturnsNotFound()
        {
            // Arrange
            _productRepo.Setup(x => x.EditProductById(It.IsAny<int>(), It.IsAny<Product>()))
                           .ReturnsAsync(It.IsAny<Product>());

            // Act
            var result = await ProductsController.EditProductById(It.IsAny<int>(), 
                                                        It.IsAny<Product>());

            // Assert
            _productRepo.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task EditProductById_ReturnsOk()
        {
            // Arrange
            var testModel = new Product() { Id = 1 };
            var testProd = new Product();
            _productRepo.Setup(x => x.EditProductById(It.IsAny<int>(), It.IsAny<Product>()))
                           .ReturnsAsync(testProd);
            var controller = ProductsController;

            // Act
            var result = await controller.EditProductById(It.IsAny<int>(),
                                                        It.IsAny<Product>());

            // Assert
            _productRepo.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task DeleteProductById_ReturnsOk()
        {
            // Arrange
            _productRepo.Setup(x => x.DeleteProduct(It.IsAny<int>()));
            var controller = ProductsController;

            // Act
            var result = await controller.DeleteReviewById(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
