using Application.Products;
using Domain;
using Moq;
using Shouldly;
using ECommerce.Core.Persistence;

namespace ECommerce.Tests.Application.Products;

public class DeleteCommandHandlerTests
{
    private readonly Mock<IEventStoreRepository<Product>> _productWriteRepositoryMock;
    private readonly DeleteProduct.Handler _handler;

    public DeleteCommandHandlerTests()
    {
        _productWriteRepositoryMock = new Mock<IEventStoreRepository<Product>>();
        _handler = new DeleteProduct.Handler(_productWriteRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenProductNotFound()
    {
        // Arrange
        var command = new DeleteProduct.Command(ProductId.Of(Guid.NewGuid()));
        var productId = Guid.NewGuid();
        _productWriteRepositoryMock.Setup(repo => repo.FetchStreamAsync(productId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeOfType<ProductNotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenFailedToDeleteProduct()
    {
        // Arrange
        var productId = ProductId.Of(Guid.NewGuid());
        var command = new DeleteProduct.Command(productId);
        var productData = new ProductData(
            "Test Product",
            "Test Description",
            Money.Of(100, Currency.USDollar.Code),
            "http://example.com/image.jpg",
            Category.Electronics
        );
        var product = Product.Create(productData);

        _productWriteRepositoryMock.Setup(repo => repo.FetchStreamAsync(productId.Value, null, It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _productWriteRepositoryMock.Setup(repo => repo.AppendEventsAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Failed to delete product"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Message.ShouldBe("Failed to delete product");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenProductIsDeleted()
    {
        // Arrange
        var productId = ProductId.Of(Guid.NewGuid());
        var command = new DeleteProduct.Command(productId);
        var productData = new ProductData(
            "Test Product",
            "Test Description",
            Money.Of(100, Currency.USDollar.Code),
            "http://example.com/image.jpg",
            Category.Electronics
        );
        var product = Product.Create(productData);

        _productWriteRepositoryMock.Setup(repo => repo.FetchStreamAsync(productId.Value, null, It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _productWriteRepositoryMock.Setup(repo => repo.AppendEventsAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<long>(0));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        _productWriteRepositoryMock.Verify(repo => repo.AppendEventsAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}