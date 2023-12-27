using CatalogApi;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OrionEShopOnContainers.Services.Catalog.API.Grpc;
using OrionEShopOnContainers.Services.Catalog.API.Infrastructure;
using OrionEShopOnContainers.Services.Catalog.API.Models;
using OrionEShopOnContainers.Services.Catalog.UnitTests.Helpers;

namespace OrionEShopOnContainers.Services.Catalog.UnitTests
{
    public class CatalogServiceTests
    {
        private readonly DbContextOptions<CatalogContext> _dbOptions;

        public CatalogServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: "in-memory")
            .Options;

            using var dbContext = new CatalogContext(_dbOptions);
            dbContext.CatalogBrands.Add(new API.Models.CatalogBrand { Id = 1, Brand = "Brand 1" });
            dbContext.CatalogTypes.Add(new API.Models.CatalogType { Id = 1, Type = "Type 1" });
            dbContext.CatalogItems.Add(new CatalogItem
            {
                Id = 1,
                Name = "Test Item",
                CatalogBrandId = 1,
                CatalogTypeId = 1,
                Description = "Test Item Description",
                AvailableStock = 10,
                MaxStockThreshold = 20,
                OnReorder = false,
                PictureFileName = "test.jpg",
                Price = 10,
                RestockThreshold = 5
            });
            dbContext.SaveChanges();
        }

        [Fact]
        public async void GetItemById_ReturnPreconditionFailed_WhenIdIsZero()
        {
            var mockLogger = new Mock<ILogger<CatalogService>>();
            var dbContext = new CatalogContext(_dbOptions);

            var service = new CatalogService(mockLogger.Object, dbContext);
            
            var context = TestServerCallContext.Create();
            var response = await service.GetItemById(new CatalogItemRequest { Id = 0 }, context);

            Assert.Equal(StatusCode.FailedPrecondition, context.Status.StatusCode);
        }

        [Fact]
        public async void GetItemById_ReturnNotFound_WhenItemDoesNotExist()
        {
            var mockLogger = new Mock<ILogger<CatalogService>>();
            var dbContext = new CatalogContext(_dbOptions);

            var service = new CatalogService(mockLogger.Object, dbContext);
            
            var context = TestServerCallContext.Create();
            var response = await service.GetItemById(new CatalogItemRequest { Id = 2 }, context);

            Assert.Equal(StatusCode.NotFound, context.Status.StatusCode);
        }

        [Fact]
        public async void GetItemById_ReturnCatalogItem_WhenItemExists()
        {
            var mockLogger = new Mock<ILogger<CatalogService>>();
            var dbContext = new CatalogContext(_dbOptions);

            var service = new CatalogService(mockLogger.Object, dbContext);
            
            var context = TestServerCallContext.Create();
            var response = await service.GetItemById(new CatalogItemRequest { Id = 1 }, context);

            Assert.Equal(StatusCode.OK, context.Status.StatusCode);
            Assert.Equal(1, response.Id);
            Assert.Equal("Test Item", response.Name);
            Assert.Equal(1, response.CatalogBrand.Id);
            Assert.Equal("Brand 1", response.CatalogBrand.Name);
            Assert.Equal(1, response.CatalogType.Id);
            Assert.Equal("Type 1", response.CatalogType.Type);
            Assert.Equal("Test Item Description", response.Description);
            Assert.Equal(10, response.AvailableStock);
            Assert.Equal(20, response.MaxStockThreshold);
            Assert.False(response.OnReorder);
            Assert.Equal("test.jpg", response.PictureFileName);
            Assert.Equal(10, response.Price);
            Assert.Equal(5, response.RestockThreshold);
        }
    }
}