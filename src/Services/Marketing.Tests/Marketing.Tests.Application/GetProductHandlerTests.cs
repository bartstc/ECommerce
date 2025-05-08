using Marketing.Infrastructure.Projections;

namespace Marketing.Tests.Application;

public class GetProductHandlerTests
{
    private readonly Mock<IEventStoreRepository<Product>> _productWriteRepository;
    private readonly GetProduct.Handler _handler;

    public GetProductHandlerTests()
    {
        _productWriteRepository = new Mock<IEventStoreRepository<Product>>();
        _handler = new GetProduct.Handler(_productWriteRepository.Object);
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

        _productWriteRepository
            .Setup(s => s.FetchLatest<ProductDetails>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProduct);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.AsT0.ShouldBe(expectedProduct);
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenProductNotFound()
    {
        var productId = Guid.NewGuid();
        var query = new GetProduct.Query(ProductId.Of(productId));

        _productWriteRepository
            .Setup(s => s.FetchLatest<ProductDetails>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductDetails)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.AsT1.Message.ShouldBe(new ProductException.NotFound().Message);
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenProductIsArchived()
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

        _productWriteRepository
            .Setup(s => s.FetchLatest<ProductDetails>(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(archivedProduct);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.AsT1.Message.ShouldBe(new ProductException.NotFound().Message);
    }
}