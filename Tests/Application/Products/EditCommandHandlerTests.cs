using Application.Products;
using Application.Products.Dtos;
using Domain;
using Domain.Errors;
using Moq;
using Shouldly;

namespace ECommerce.Tests.Application.Products
{
    public class EditCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Edit.Handler _handler;

        public EditCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new Edit.Handler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenProductNotFound()
        {
            var command = new Edit.Command(Guid.NewGuid(), new CreateProductDto(
                "Updated Product",
                "Updated Description",
                new CreatePriceDto(150, "USD"),
                new CreateRatingDto(4.5, 10),
                "updated-image.jpg",
                "electronics"
            ));
            _productRepositoryMock.Setup(repo => repo.GetProduct(command.Id)).ReturnsAsync((Product)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBe(ProductsError.ProductNotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFailedToUpdateProduct()
        {
            var productId = Guid.NewGuid();
            var command = new Edit.Command(productId, new CreateProductDto(
                "Updated Product",
                "Updated Description",
                new CreatePriceDto(150, "USD"),
                new CreateRatingDto(4.5, 10),
                "updated-image.jpg",
                "electronics"
            ));
            var product = new Product { Id = productId, Title = "Old Product", Price = new Money(100, Currency.USD) };

            _productRepositoryMock.Setup(repo => repo.GetProduct(productId)).ReturnsAsync(product);
            _productRepositoryMock.Setup(repo => repo.Complete()).ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBe(ProductsError.FailedToUpdateProduct);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenProductIsUpdated()
        {
            var productId = Guid.NewGuid();
            var command = new Edit.Command(productId, new CreateProductDto(
                "Updated Product",
                "Updated Description",
                new CreatePriceDto(150, "USD"),
                new CreateRatingDto(4.5, 10),
                "updated-image.jpg",
                "electronics"
            ));
            var product = new Product { Id = productId, Title = "Old Product", Price = new Money(100, Currency.USD) };

            _productRepositoryMock.Setup(repo => repo.GetProduct(productId)).ReturnsAsync(product);
            _productRepositoryMock.Setup(repo => repo.Complete()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
            _productRepositoryMock.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);
        }
    }
}