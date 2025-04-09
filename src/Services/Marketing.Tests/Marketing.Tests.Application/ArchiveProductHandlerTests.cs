namespace Marketing.Tests.Application;

public class ArchiveProductHandlerTests
{
    private readonly Mock<IEventStoreRepository<Product>> _productWriteRepository;
    private readonly ArchiveProduct.Handler _handler;

    public ArchiveProductHandlerTests()
    {
        _productWriteRepository = new Mock<IEventStoreRepository<Product>>();
        _handler = new ArchiveProduct.Handler(_productWriteRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ArchiveProduct_WhenProductExists()
    {
        var productId = Guid.NewGuid();
        var command = new ArchiveProduct.Command(ProductId.Of(productId));
        var product = Product.Create(new ProductData(ProductId.Of(productId)));
        var eventStream =
            new DummyEventStream<Product>(productId, 0, new List<IEvent>(), product, CancellationToken.None);

        _productWriteRepository
            .Setup(r => r.FetchForWriting<Product>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventStream);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _productWriteRepository.Verify(r => r.AppendEvents(It.IsAny<Product>()), Times.Once);
        _productWriteRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductNotFound()
    {
        var productId = Guid.NewGuid();
        var command = new ArchiveProduct.Command(ProductId.Of(productId));
        var eventStream = new DummyEventStream<Product>(productId, 0, new List<IEvent>(), null, CancellationToken.None);

        _productWriteRepository
            .Setup(r => r.FetchForWriting<Product>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventStream);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Message.ShouldBe(new ProductNotFoundException().Message);
        _productWriteRepository.Verify(r => r.AppendEvents(It.IsAny<Product>()), Times.Never);
        _productWriteRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}