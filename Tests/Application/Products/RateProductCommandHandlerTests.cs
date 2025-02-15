using Application.Products;
using Application.Products.Dtos;
using Application.Products.Exceptions;
using Domain;
using ECommerce.Core.Persistence;
using Moq;
using Shouldly;

namespace ECommerce.Tests.Application.Products
{
    public class RateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RateProduct.Handler _handler;

        public RateProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new RateProduct.Handler(_productRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenProductNotFound()
        {
            var command = new RateProduct.Command(Guid.NewGuid(), new RateProductDto(5));
            _productRepositoryMock.Setup(repo => repo.GetProduct(ProductId.Of(command.ProductId))).ReturnsAsync((Product)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBeOfType<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenRatingIsSuccessful()
        {
            var productId = Guid.NewGuid();
            var command = new RateProduct.Command(productId, new RateProductDto(5));
            var productData = new ProductData(
                productId,
                "Test Product",
                "Test Description",
                Money.Of(100, Currency.USDollar.Code),
                Rating.Of(0, 0),
                "test-image.jpg",
                Category.Electronics,
                DateTime.UtcNow,
                null
            );
            var product = Product.Create(productData);

            _productRepositoryMock.Setup(repo => repo.GetProduct(ProductId.Of(productId))).ReturnsAsync(product);
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
            _productRepositoryMock.Verify(repo => repo.UpdateProduct(product), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFailedToComplete()
        {
            var productId = Guid.NewGuid();
            var command = new RateProduct.Command(productId, new RateProductDto(5));
            var productData = new ProductData(
                productId,
                "Test Product",
                "Test Description",
                Money.Of(100, Currency.USDollar.Code),
                Rating.Of(0, 0),
                "test-image.jpg",
                Category.Electronics,
                DateTime.UtcNow,
                null
            );
            var product = Product.Create(productData);

            _productRepositoryMock.Setup(repo => repo.GetProduct(ProductId.Of(productId))).ReturnsAsync(product);
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBeOfType<FailedToRateProductException>();
        }
    }
}