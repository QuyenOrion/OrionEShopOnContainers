using Client;
using GrpcBasket;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddGrpcClient<Basket.BasketClient>(o =>
{
    o.Address = new Uri("https://localhost:5001");
});

var host = builder.Build();
host.Run();
