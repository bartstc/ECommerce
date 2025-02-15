using Application.Products;
using Application.Products.Dtos;
using Application.Products.Exceptions;
using ECommerce.Core.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace API.Controllers.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetProducts_Should_ReturnOkResult()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto(
                    Guid.NewGuid(),
                    "Test Product",
                    "Test Description",
                    new MoneyDto(100, "USD"),
                    new RatingDto(5, 10),
                    "Test Category",
                    "Test Brand",
                    DateTime.UtcNow,
                    null)
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProducts.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<List<ProductDto>>.Success(products));

            // Act
            var result = await _controller.GetProducts();

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.ShouldBe(products);
        }

        [Fact]
        public async Task GetProduct_Should_ReturnNotFound_WhenProductNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProduct.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductDto>.Failure(new ProductNotFoundException()));

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            result.ShouldBeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Value.ShouldBe("Product not found");
        }

        [Fact]
        public async Task GetProduct_Should_ReturnOkResult_WhenProductIsFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new ProductDto(
                productId,
                "Test Product",
                "Test Description",
                new MoneyDto(100, "USD"),
                new RatingDto(5, 10),
                "Test Category",
                "Test Brand",
                DateTime.UtcNow,
                null
            );
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProduct.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductDto>.Success(product));

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.ShouldBe(product);
        }

        [Fact]
        public async Task CreateProduct_Should_ReturnBadRequest_WhenFailedToCreateProduct()
        {
            // Arrange
            var productDto = new CreateProductDto(
                "Test Product",
                "Test Description",
                new CreatePriceDto(100, "USD"),
                "Test Category",
                "Test Brand"
            );
            _mediatorMock.Setup(m => m.Send(It.IsAny<AddProduct.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Unit>.Failure(new FailedToCreateProductException()));

            // Act
            var result = await _controller.AddProduct(productDto);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.ShouldBe("Failed to create the product");
        }

        [Fact]
        public async Task CreateProduct_Should_ReturnOkResult_WhenProductIsCreatedSuccessfully()
        {
            // Arrange
            var productDto = new CreateProductDto(
                "Test Product",
                "Test Description",
                new CreatePriceDto(100, "USD"),
                "Test Category",
                "Test Brand"
            );
            _mediatorMock.Setup(m => m.Send(It.IsAny<AddProduct.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Unit>.Success(Unit.Value));

            // Act
            var result = await _controller.AddProduct(productDto);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EditProduct_Should_ReturnNotFound_WhenProductNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new CreateProductDto(
                "Updated Product",
                "Updated Description",
                new CreatePriceDto(150, "USD"),
                "Updated Category",
                "Updated Brand"
            );
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProduct.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Unit>.Failure(new ProductNotFoundException()));

            // Act
            var result = await _controller.UpdateProduct(productId, productDto);

            // Assert
            result.ShouldBeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Value.ShouldBe("Product not found");
        }

        [Fact]
        public async Task EditProduct_Should_ReturnOkResult_WhenProductIsUpdatedSuccessfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new CreateProductDto(
                "Updated Product",
                "Updated Description",
                new CreatePriceDto(150, "USD"),
                "Updated Category",
                "Updated Brand"
            );
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProduct.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Unit>.Success(Unit.Value));

            // Act
            var result = await _controller.UpdateProduct(productId, productDto);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteProduct_Should_ReturnBadRequest_WhenFailedToDeleteProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProduct.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Unit>.Failure(new FailedToDeleteProductException()));

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.ShouldBe("Failed to delete the product");
        }

        [Fact]
        public async Task DeleteProduct_Should_ReturnOkResult_WhenProductIsDeletedSuccessfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProduct.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Unit>.Success(Unit.Value));

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }
    }
}