using MediatR;

namespace Marketing.Tests.Application;

public class CreateProductHandlerTests
{
    private readonly Mock<IEventStoreRepository<Product>> _productWriteRepository;
    private readonly Mock<IQuerySession> _querySession;
    private readonly CreateProduct.Handler _handler;

    public CreateProductHandlerTests()
    {
        _productWriteRepository = new Mock<IEventStoreRepository<Product>>();
        _querySession = new Mock<IQuerySession>();
        _handler = new CreateProduct.Handler(_productWriteRepository.Object, _querySession.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnProductAlreadyExists_WhenProductExists()
    {
        var productId = Guid.NewGuid();
        var productDto = new CreateProductDto(productId);
        var command = new CreateProduct.Command(productDto);

        var existingStreamState = new StreamState
        {
            Id = productId,
            Version = 1
        };

        _querySession
            .Setup(session => session.Events.FetchStreamStateAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStreamState);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.AsT3.Message.ShouldBe("Product already exists");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenProductIsCreated()
    {
        var productId = Guid.NewGuid();
        var productDto = new CreateProductDto(productId);
        var command = new CreateProduct.Command(productDto);

        _productWriteRepository.Setup(repo => repo.AppendEvents(It.IsAny<Product>()));
        _productWriteRepository
            .Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _querySession
            .Setup(session => session.Events.FetchStreamStateAsync(productId, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult((StreamState)null));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.AsT0.ShouldBeOfType<Unit>();

        _productWriteRepository.Verify(repo => repo.AppendEvents(It.IsAny<Product>()), Times.Once);
        _productWriteRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}