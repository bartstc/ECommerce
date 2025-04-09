﻿
namespace Marketing.Tests.Application;

public class RateProductHandlerTests
{
    private readonly Mock<IEventStoreRepository<Product>> _productWriteRepository;
    private readonly RateProduct.Handler _handler;

    public RateProductHandlerTests()
    {
        _productWriteRepository = new Mock<IEventStoreRepository<Product>>();
        _handler = new RateProduct.Handler(_productWriteRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_RateProduct_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var rateDto = new RateProductDto(5);
        var command = new RateProduct.Command(ProductId.Of(productId), rateDto);
        var product = Product.Create(new ProductData(ProductId.Of(productId)));
        var eventStream =
            new DummyEventStream<Product>(productId, 0, new List<IEvent>(), product, CancellationToken.None);

        _productWriteRepository
            .Setup(r => r.FetchForWriting<Product>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventStream);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        _productWriteRepository.Verify(r => r.AppendEvents(It.IsAny<Product>()), Times.Once);
        _productWriteRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var rateDto = new RateProductDto(5);
        var command = new RateProduct.Command(ProductId.Of(productId), rateDto);
        var eventStream = new DummyEventStream<Product>(productId, 0, new List<IEvent>(), null, CancellationToken.None);

        _productWriteRepository
            .Setup(r => r.FetchForWriting<Product>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventStream);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Message.ShouldBe(new ProductNotFoundException().Message);
        _productWriteRepository.Verify(r => r.AppendEvents(It.IsAny<Product>()), Times.Never);
        _productWriteRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}