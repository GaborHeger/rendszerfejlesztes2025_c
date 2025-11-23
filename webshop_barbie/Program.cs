using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using webshop_barbie.Data;
using webshop_barbie.Repository;
using webshop_barbie.Repository.Interfaces;
using webshop_barbie.Service;
using webshop_barbie.Service.Interfaces;

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
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductService, ProductService>();

// JWT autentikáció hozzáadása
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };
});

//Swagger konfiguráció JWT támogatással
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Webshop API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Írd be: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

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
    // Törli az adatbázist
    await db.Database.EnsureDeletedAsync();
    // Újra létrehozza a migrációk alapján
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

app.UseAuthentication();

// Autorizációs middlewares
app.UseAuthorization();

// CORS engedélyezése
app.UseCors("AllowAll");

// Controller végpontok engedélyezése
app.MapControllers();

// Alkalmazás indítása
app.Run();


