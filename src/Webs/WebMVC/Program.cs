using OrionEShopOnContainers.Services.Service.Common;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddHttpClientServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseServiceDefault();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}");

app.Run();
