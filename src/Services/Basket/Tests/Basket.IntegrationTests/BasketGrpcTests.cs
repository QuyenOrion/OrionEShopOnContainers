using Basket.IntegrationTests.Helpers;
using OrionEShopOnContainers.Services.Basket.API;
using Xunit.Abstractions;
using GrpcBasket;

namespace Basket.IntegrationTests
{
    public class BasketGrpcTests
    {
        //public BasketGrpcTests(GrpcTestFixture<Startup> fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        //{
        //}

        [Fact]
        public void GetBasketById_ThrowNotFoundException_WhenBasketNotExists()
        {
            var client = new Basket.BasketClient(Channel);
        }
    }
}