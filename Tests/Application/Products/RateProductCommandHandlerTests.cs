using Application.Interfaces;
using Application.Products;
using Application.Products.Dtos;
using Domain;
using Domain.Errors;
using Moq;
using Shouldly;

namespace ECommerce.Tests.Application.Products
{
    public class RateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IStoreRepository> _storeRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RateProduct.Handler _handler;

        public RateProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _storeRepositoryMock = new Mock<IStoreRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new RateProduct.Handler(_productRepositoryMock.Object, _storeRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenProductNotFound()
        {
            var command = new RateProduct.Command(Guid.NewGuid(), new RateProductDto(5));
            _productRepositoryMock.Setup(repo => repo.GetProduct(command.Id)).ReturnsAsync((Product)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBe(ProductsError.ProductNotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenStoreNotFound()
        {
            var productId = Guid.NewGuid();
            var command = new RateProduct.Command(productId, new RateProductDto(5));
            var product = new Product { Id = productId, StoreId = Guid.NewGuid(), Rating = new Rating(0, 0), Price = new Money(0, Currency.USD) };

            _productRepositoryMock.Setup(repo => repo.GetProduct(productId)).ReturnsAsync(product);
            _storeRepositoryMock.Setup(repo => repo.GetStore(product.StoreId)).ReturnsAsync((Store)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBe(ProductsError.StoreNotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenRatingIsSuccessful()
        {
            var productId = Guid.NewGuid();
            var storeId = Guid.NewGuid();
            var command = new RateProduct.Command(productId, new RateProductDto(5));
            var product = new Product { Id = productId, StoreId = storeId, Rating = new Rating(0, 0), Price = new Money(0, Currency.USD) };
            var store = new Store { Id = storeId, Rating = new Rating(0, 0) };

            _productRepositoryMock.Setup(repo => repo.GetProduct(productId)).ReturnsAsync(product);
            _storeRepositoryMock.Setup(repo => repo.GetStore(storeId)).ReturnsAsync(store);
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
            _productRepositoryMock.Verify(repo => repo.UpdateProduct(product), Times.Once);
            _storeRepositoryMock.Verify(repo => repo.UpdateStore(store), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenFailedToComplete()
        {
            var productId = Guid.NewGuid();
            var storeId = Guid.NewGuid();
            var command = new RateProduct.Command(productId, new RateProductDto(5));
            var product = new Product { Id = productId, StoreId = storeId, Rating = new Rating(0, 0), Price = new Money(0, Currency.USD) };
            var store = new Store { Id = storeId, Rating = new Rating(0, 0) };

            _productRepositoryMock.Setup(repo => repo.GetProduct(productId)).ReturnsAsync(product);
            _storeRepositoryMock.Setup(repo => repo.GetStore(storeId)).ReturnsAsync(store);
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBe(ProductsError.FailedToRateProduct);
        }
    }
}