using Application.Products;
using Application.Products.Dtos;
using Application.Products.Exceptions;
using Domain;
using Moq;
using Shouldly;

namespace ECommerce.Tests.Application.Products
{
    public class CreateCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Create.Handler _handler;

        public CreateCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new Create.Handler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFailedToCreateProduct()
        {
            var command = new Create.Command(new CreateProductDto(
                "Test Product",
                "Test Description",
                new CreatePriceDto(100, "USD"),
                new CreateRatingDto(4.5, 10),
                "test-image.jpg",
                "electronics"
            ));
            _productRepositoryMock.Setup(repo => repo.Complete()).ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBeOfType<FailedToCreateProductException>();
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenProductIsCreated()
        {
            var command = new Create.Command(new CreateProductDto(
                "Test Product",
                "Test Description",
                new CreatePriceDto(100, "USD"),
                new CreateRatingDto(4.5, 10),
                "test-image.jpg",
                "electronics"
            ));
            _productRepositoryMock.Setup(repo => repo.Complete()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
            _productRepositoryMock.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Once);
        }
    }
}