// using Moq;
// using System.Reflection;
// using System.Threading.Tasks;
// using Xunit;
// using FluentAssertions;
// using DVP.Tasks.Infrastructure.Finder.Products;
// using DVP.Tasks.Domain.AggregatesModel.ProductAggregate;
// using DVP.Tasks.Infrastructure;
// using DVP.Tasks.Infraestructure.Services;
// using DVP.Tasks.Infraestructure.Services.MockApi;

// public class ProductFinderTests
// {
//     private readonly Mock<DVPContext> _mockContext;
//     private readonly Mock<IProductStateCacheService> _mockCache;
//     private readonly Mock<IDiscountService> _mockDiscountService;
//     private readonly ProductFinder _productFinder;

//     public ProductFinderTests()
//     {
//         _mockContext = new Mock<DVPContext>();
//         _mockCache = new Mock<IProductStateCacheService>();
//         _mockDiscountService = new Mock<IDiscountService>();
//         _productFinder = new ProductFinder(_mockContext.Object, _mockCache.Object, _mockDiscountService.Object);
//     }

//     [Fact]
//     public async Task GetProductDtoByIdAsync_ShouldReturnProductDto_WhenProductExists()
//     {
//         // Arrange
//         var productId = Guid.NewGuid();
//         var product = new Product
//         {
//             Name = "Test Product",
//             Price = 100.00m,
//             Status = 1,
//             Stock = 10,
//             Description = "Test Description"
//         };

//         var idProperty = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//         idProperty.SetValue(product, productId);

//         _mockContext.Setup(c => c.Products.FindAsync(productId))
//             .ReturnsAsync(product);

//         _mockCache.Setup(c => c.GetProductStatus(It.IsAny<int>())).Returns("Available");
//         _mockDiscountService.Setup(d => d.GetDiscountAsync(productId)).ReturnsAsync(10);

//         // Act
//         var result = await _productFinder.GetProductDtoByIdAsync(productId);

//         // Assert
//         result.Should().NotBeNull();
//         result.ProductId.Should().Be(productId);
//         result.Name.Should().Be("Test Product");
//         result.Price.Should().Be(100.00m);
//         result.StatusName.Should().Be("Available");
//         result.Stock.Should().Be(10);
//         result.Description.Should().Be("Test Description");
//         result.FinalPrice.Should().Be(90.00m); // Price after 10% discount

//         // Assert using reflection
//         var actualProductId = idProperty.GetValue(product);
//         actualProductId.Should().Be(productId);
//     }

//     [Fact]
//     public async Task GetProductDtoByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
//     {
//         // Arrange
//         var productId = Guid.NewGuid();

//         _mockContext.Setup(c => c.Products.FindAsync(productId))
//             .ReturnsAsync((Product)null);

//         // Act
//         var result = await _productFinder.GetProductDtoByIdAsync(productId);

//         // Assert
//         Assert.Null(result);
//     }

//     [Fact]
//     public async Task FindByIdAsync_ShouldReturnProduct_WhenProductExists()
//     {
//         // Arrange
//         var productId = Guid.NewGuid();
//         var product = new Product();

//         // Set the Id property using reflection
//         var idProperty = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//         idProperty.SetValue(product, productId);

//         _mockContext.Setup(c => c.Products.FindAsync(productId))
//             .ReturnsAsync(product);

//         // Act
//         var result = await _productFinder.FindByIdAsync(productId);

//         // Assert
//         Assert.NotNull(result);
//         // Verify Id property using reflection
//         var resultId = (Guid)idProperty.GetValue(result);
//         Assert.Equal(productId, resultId);
//     }

//     [Fact]
//     public async Task FindByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
//     {
//         // Arrange
//         var productId = Guid.NewGuid();

//         _mockContext.Setup(c => c.Products.FindAsync(productId))
//             .ReturnsAsync((Product)null);

//         // Act
//         var result = await _productFinder.FindByIdAsync(productId);

//         // Assert
//         Assert.Null(result);
//     }
// }
