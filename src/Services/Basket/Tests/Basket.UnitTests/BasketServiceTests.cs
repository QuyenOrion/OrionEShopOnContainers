namespace Basket.UnitTests
{
    public class BasketServiceTests
    {
        [Fact]
        public void GetBasketById_ReturnNotFoundStatus_WhenBasketNotExists()
        {
            var mockRepository = new Mock<IBasketRepository>();
            mockRepository.Setup(x => x.GetBasketAsync("2")).Returns(Task.FromResult(new CustomerBasket { BuyerId = "2" }));
            var mockLogger = new Mock<ILogger<BasketService>>();

            var service = new BasketService(mockRepository.Object, mockLogger.Object);
            TestServerCallContext context = TestServerCallContext.Create();
            var response = service.GetBasketById(new BasketRequest { Id = "1" }, context);

            Assert.Equal(StatusCode.NotFound, context.Status.StatusCode);
        }

        [Fact]
        public async void GetBasketById_ReturnOkStatus_WhenBasketExists()
        {
            var mockRepository = new Mock<IBasketRepository>();
            mockRepository.Setup(x => x.GetBasketAsync("1")).Returns(Task.FromResult(new CustomerBasket { BuyerId = "1"}));
            var mockLogger = new Mock<ILogger<BasketService>>();

            var service = new BasketService(mockRepository.Object, mockLogger.Object);
            TestServerCallContext context = TestServerCallContext.Create();
            var response = await service.GetBasketById(new BasketRequest { Id = "1" }, context);

            Assert.Equal(StatusCode.OK, context.Status.StatusCode);
            Assert.NotNull(response);
            Assert.Equal("1", response.Buyerid);
            Assert.Empty(response.Items);
        }

        [Fact]
        public async void GetBasketById_ReturnBasketWithItems_WhenBasketExists()
        {
            var mockRepository = new Mock<IBasketRepository>();
            mockRepository.Setup(x => x.GetBasketAsync("1")).Returns(Task.FromResult(new CustomerBasket { BuyerId = "1", Items = new List<BasketItem> { new() { Id = "a", PictureUrl = "c", ProductName = "b" } } }));
            var mockLogger = new Mock<ILogger<BasketService>>();

            var service = new BasketService(mockRepository.Object, mockLogger.Object);
            TestServerCallContext context = TestServerCallContext.Create();
            var response = await service.GetBasketById(new BasketRequest { Id = "1" }, context);

            Assert.Equal(StatusCode.OK, context.Status.StatusCode);
            Assert.NotNull(response);
            Assert.Equal("1", response.Buyerid);
            Assert.Single(response.Items);
            Assert.Equal("a", response.Items[0].Id);
        }

        [Fact]
        public async void UpdateBasket_ReturnInternalStatus_WhenBasketCantSave()
        {
            var mockRepository = new Mock<IBasketRepository>();
            mockRepository.Setup(x => x.UpdateBasketAsync(It.IsAny<CustomerBasket>())).Returns(Task.FromResult<CustomerBasket>(null));
            var mockLogger = new Mock<ILogger<BasketService>>();

            var service = new BasketService(mockRepository.Object, mockLogger.Object);
            TestServerCallContext context = TestServerCallContext.Create();
            var response = await service.UpdateBasket(new CustomerBasketRequest { Buyerid = "1" }, context);

            Assert.Equal(StatusCode.Internal, context.Status.StatusCode);
        }

        [Fact]
        public async void UpdateBasket_ReturnOkStatus_WhenBasketSaved()
        {
            var mockRepository = new Mock<IBasketRepository>();
            mockRepository.Setup(x => x.UpdateBasketAsync(It.IsAny<CustomerBasket>())).Returns(Task.FromResult(new CustomerBasket { BuyerId = "1" }));
            var mockLogger = new Mock<ILogger<BasketService>>();

            var service = new BasketService(mockRepository.Object, mockLogger.Object);
            TestServerCallContext context = TestServerCallContext.Create();
            var response = await service.UpdateBasket(new CustomerBasketRequest { Buyerid = "1" }, context);

            Assert.Equal(StatusCode.OK, context.Status.StatusCode);
            mockRepository.Verify(x => x.UpdateBasketAsync(It.IsAny<CustomerBasket>()), Times.Once);
        }
    }
}