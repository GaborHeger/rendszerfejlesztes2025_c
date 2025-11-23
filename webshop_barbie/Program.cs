using Microsoft.EntityFrameworkCore;
using webshop_barbie.Data;
using webshop_barbie.Service.Interfaces;
using webshop_barbie.Service;
using webshop_barbie.Repository.Interfaces;
using webshop_barbie.Repository;

var builder = WebApplication.CreateBuilder(args);

// CONTROLLEREK HOZZÁADÁSA
builder.Services.AddControllers();

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ADATBÁZIS KONFIGURÁCIÓ (PostgreSQL + EF Core)
// A DefaultConnection-t a appsettings.json-ból olvassa ki.
builder.Services.AddDbContext<WebshopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS BEÁLLÍTÁS
// A frontend (HTML+JS localhost) így éri el a backendet.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()   // bárhonnan jöhet kérés (fejlesztés alatt)
            .AllowAnyMethod()   // GET, POST, PUT, DELETE
            .AllowAnyHeader()); // bármilyen header engedélyezett
});

// SERVICE ÉS REPOSITORY RÉTEGEK REGISZTRÁLÁSA
// MUSZÁJ, különben a controllerek nem tudják őket használni.

// User
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Product
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Cart
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService,  UserService>();

// Favorite
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();

// Order
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// AUTOMATIKUS ADATBÁZIS MIGRÁLÁS INDULÁSKOR
// A frontend fejlesztőnek NEM kell migrációt futtatnia.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WebshopContext>();
    db.Database.Migrate();
    SeedData.Initialize(db);
}

// FEJLESZTÉSI KÖRNYEZET – SWAGGER
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Globális hibakezelő middleware
app.UseMiddleware<webshop_barbie.Middleware.GlobalExceptionHandlerMiddleware>();

//automatikusan átirányítja a HTTP kéréseket HTTPS-re
app.UseHttpsRedirection();

// Autorizációs middlewares
app.UseAuthorization();

// CORS engedélyezése
app.UseCors("AllowAll");

// Controller végpontok engedélyezése
app.MapControllers();

// Alkalmazás indítása
app.Run();


