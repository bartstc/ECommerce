using Application.Products;
using Application.Products.Exceptions;
using Domain;
using Moq;
using Shouldly;

namespace ECommerce.Tests.Application.Products
{
    public class DeleteCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly DeleteProduct.Handler _handler;

        public DeleteCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new DeleteProduct.Handler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenProductNotFound()
        {
            var command = new DeleteProduct.Command(Guid.NewGuid());
            _productRepositoryMock.Setup(repo => repo.GetProduct(ProductId.Of(command.Id))).ReturnsAsync((Product)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBeOfType<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFailedToDeleteProduct()
        {
            var productId = Guid.NewGuid();
            var command = new DeleteProduct.Command(productId);
            var productData = new ProductData(
                productId,
                "Test Product",
                "Test Description",
                Money.Of(100, Currency.USDollar.Code),
                Rating.Of(5, 1),
                "http://example.com/image.jpg",
                Category.Electronics,
                DateTime.UtcNow,
                null
            );
            var product = Product.Create(productData);

            _productRepositoryMock.Setup(repo => repo.GetProduct(ProductId.Of(productId))).ReturnsAsync(product);
            _productRepositoryMock.Setup(repo => repo.Complete()).ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBeOfType<FailedToDeleteProductException>();
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenProductIsDeleted()
        {
            var productId = Guid.NewGuid();
            var command = new DeleteProduct.Command(productId);
            var productData = new ProductData(
                productId,
                "Test Product",
                "Test Description",
                Money.Of(100, Currency.USDollar.Code),
                Rating.Of(5, 1),
                "http://example.com/image.jpg",
                Category.Electronics,
                DateTime.UtcNow,
                null
            );
            var product = Product.Create(productData);

            _productRepositoryMock.Setup(repo => repo.GetProduct(ProductId.Of(productId))).ReturnsAsync(product);
            _productRepositoryMock.Setup(repo => repo.Complete()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
            _productRepositoryMock.Verify(repo => repo.DeleteProduct(It.IsAny<Product>()), Times.Once);
        }
    }
}