using Application.Products;
using Application.Products.Dtos;
using Domain;
using Moq;
using Shouldly;
using ECommerce.Core.Persistence;

namespace ECommerce.Tests.Application.Products;

public class AddCommandHandlerTests
{
    private readonly Mock<IEventStoreRepository<Product>> _productWriteRepositoryMock;
    private readonly AddProduct.Handler _handler;

    public AddCommandHandlerTests()
    {
        _productWriteRepositoryMock = new Mock<IEventStoreRepository<Product>>();
        _handler = new AddProduct.Handler(_productWriteRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenFailedToCreateProduct()
    {
        // Arrange
        var command = new AddProduct.Command(new CreateProductDto(
            "Test Product",
            "Test Description",
            new CreatePriceDto(100, "USD"),
            "test-image.jpg",
            "electronics"
        ));
        _productWriteRepositoryMock.Setup(repo => repo.AppendEventsAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Failed to create product"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Message.ShouldBe("Failed to create product");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenProductIsCreated()
    {
        // Arrange
        var command = new AddProduct.Command(new CreateProductDto(
            "Test Product",
            "Test Description",
            new CreatePriceDto(100, "USD"),
            "test-image.jpg",
            "electronics"
        ));
        _productWriteRepositoryMock.Setup(repo => repo.AppendEventsAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<long>(0));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        _productWriteRepositoryMock.Verify(repo => repo.AppendEventsAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
