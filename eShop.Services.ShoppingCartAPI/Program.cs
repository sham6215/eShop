using AutoMapper;
using eShop.MessageBus;
using eShop.Services.ShoppingCartAPI.Data;
using eShop.Services.ShoppingCartAPI.Extensions;
using eShop.Services.ShoppingCartAPI.Mapping;
using eShop.Services.ShoppingCartAPI.Service.IService;
using eShop.Services.ShoppingCartAPI.Services;
using eShop.Services.ShoppingCartAPI.Services.IService;
using eShop.Services.ShoppingCartAPI.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Automapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient(StaticData.ProductHttpClient, 
    u => u.BaseAddress = new Uri(builder.Configuration[StaticData.ProductApiConfig]))
    .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>(); // Message handler for token adding
builder.Services.AddHttpClient(StaticData.CouponHttpClient,
    u => u.BaseAddress = new Uri(builder.Configuration[StaticData.CouponApiConfig]))
    .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>(); // Message handler for token adding

// Services DI setup

// ShoppingCart service
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.AddAppAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

ApplyMigrations();

app.Run();

void ApplyMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetService<AppDbContext>();

        if (db.Database.GetPendingMigrations().Count() > 0)
        {
            db.Database.Migrate();
        }
    }
}
