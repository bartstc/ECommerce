using Application.Products;
using Application.Products.Dtos;
using Application.Products.Exceptions;
using Domain;
using Moq;
using Shouldly;

namespace ECommerce.Tests.Application.Products
{
    public class EditCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly UpdateProduct.Handler _handler;

        public EditCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new UpdateProduct.Handler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenProductNotFound()
        {
            var command = new UpdateProduct.Command(Guid.NewGuid(), new CreateProductDto(
                "Updated Product",
                "Updated Description",
                new CreatePriceDto(150, "USD"),
                new CreateRatingDto(4.5, 10),
                "updated-image.jpg",
                "electronics"
            ));
            _productRepositoryMock.Setup(repo => repo.GetProduct(ProductId.Of(command.ProductId))).ReturnsAsync((Product)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBeOfType<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFailedToUpdateProduct()
        {
            var productId = Guid.NewGuid();
            var command = new UpdateProduct.Command(productId, new CreateProductDto(
                "Updated Product",
                "Updated Description",
                new CreatePriceDto(150, "USD"),
                new CreateRatingDto(4.5, 10),
                "updated-image.jpg",
                "electronics"
            ));
            var productData = new ProductData(
                productId,
                "Old Product",
                "Old Description",
                Money.Of(100, Currency.USDollar.Code),
                Rating.Of(4, 5),
                "old-image.jpg",
                Category.Electronics,
                DateTime.UtcNow,
                null
            );
            var product = Product.Create(productData);

            _productRepositoryMock.Setup(repo => repo.GetProduct(ProductId.Of(productId))).ReturnsAsync(product);
            _productRepositoryMock.Setup(repo => repo.Complete()).ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBeOfType<FailedToUpdateProductException>();
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenProductIsUpdated()
        {
            var productId = Guid.NewGuid();
            var command = new UpdateProduct.Command(productId, new CreateProductDto(
                "Updated Product",
                "Updated Description",
                new CreatePriceDto(150, "USD"),
                new CreateRatingDto(4.5, 10),
                "updated-image.jpg",
                "electronics"
            ));
            var productData = new ProductData(
                productId,
                "Old Product",
                "Old Description",
                Money.Of(100, Currency.USDollar.Code),
                Rating.Of(4, 5),
                "old-image.jpg",
                Category.Electronics,
                DateTime.UtcNow,
                null
            );
            var product = Product.Create(productData);

            _productRepositoryMock.Setup(repo => repo.GetProduct(ProductId.Of(productId))).ReturnsAsync(product);
            _productRepositoryMock.Setup(repo => repo.Complete()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
            _productRepositoryMock.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);
        }
    }
}