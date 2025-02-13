using Application.Products;
using Domain;
using Domain.Errors;
using Moq;
using Shouldly;

namespace ECommerce.Tests.Application.Products
{
    public class DetailsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Details.Handler _handler;

        public DetailsQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new Details.Handler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenProductNotFound()
        {
            var query = new Details.Query(Guid.NewGuid());
            _productRepositoryMock.Setup(repo => repo.GetProduct(query.Id)).ReturnsAsync((Product)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBe(ProductsError.ProductNotFound);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenProductIsFound()
        {
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Title = "Test Product",
                Description = "Test Description",
                Price = new Money(100, Currency.USD),
                Rating = new Rating(4.5, 10),
                Image = "test-image.jpg",
                Category = Category.Electronics,
                AddedAt = DateTime.UtcNow
            };
            var query = new Details.Query(productId);

            _productRepositoryMock.Setup(repo => repo.GetProduct(productId)).ReturnsAsync(product);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldNotBeNull();
            result.Value.Id.ShouldBe(productId);
            result.Value.Title.ShouldBe("Test Product");
        }
    }
}