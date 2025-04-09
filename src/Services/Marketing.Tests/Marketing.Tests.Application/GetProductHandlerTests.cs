using Marketing.Infrastructure.Projections;

namespace Marketing.Tests.Application;

public class GetProductHandlerTests
{
    private readonly Mock<IQuerySession> _querySession;
    private readonly GetProduct.Handler _handler;

    public GetProductHandlerTests()
    {
        _querySession = new Mock<IQuerySession>();
        _handler = new GetProduct.Handler(_querySession.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnProduct_WhenProductExists()
    {
        var productId = Guid.NewGuid();
        var query = new GetProduct.Query(ProductId.Of(productId));
        var expectedProduct = new ProductDetails(
            productId,
            5,
            1,
            ProductStatus.Active,
            DateTime.Now,
            null,
            null);

        _querySession.Setup(s => s.LoadAsync<ProductDetails>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProduct);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedProduct);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductNotFound()
    {
        var productId = Guid.NewGuid();
        var query = new GetProduct.Query(ProductId.Of(productId));

        _querySession.Setup(s => s.LoadAsync<ProductDetails>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductDetails)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Message.ShouldBe(new ProductNotFoundException().Message);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenProductIsArchived()
    {
        var productId = Guid.NewGuid();
        var query = new GetProduct.Query(ProductId.Of(productId));
        var archivedProduct = new ProductDetails(
            productId,
            5,
            1,
            ProductStatus.Archived,
            DateTime.Now,
            null,
            null);

        _querySession.Setup(s => s.LoadAsync<ProductDetails>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(archivedProduct);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Message.ShouldBe(new ProductNotFoundException().Message);
    }
}