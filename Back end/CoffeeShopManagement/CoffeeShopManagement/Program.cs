using CoffeeShopManagement.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Data.Repositories;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Business.Services;
using CoffeeShopManagement.Data.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CoffeeShopDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Validates that the token was issued by a trusted issuer
            ValidateAudience = true, // Validates that the token was created for a specific audience
            ValidateLifetime = true, // Ensures the token hasn't expired
            ValidateIssuerSigningKey = true, // Verifies the token’s signature
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Replace with your issuer (who issued the token)
            ValidAudience = builder.Configuration["Jwt:Audience"], // Replace with your audience (who the token is intended for)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])), // The signing key for token validation
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Define JWT Bearer token in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",                    // The name of the header parameter in the request
        Type = SecuritySchemeType.Http,            // The type of the security scheme (HTTP for Bearer tokens)
        Scheme = "Bearer",                         // The specific HTTP scheme used (Bearer)
        BearerFormat = "JWT",                      // The format for the token (JWT in this case)
        In = ParameterLocation.Header,             // Where the token should be sent (header in this case)
        Description = "Enter your token",  // Description shown in Swagger UI
    });

    // Set up the authorization requirement for all endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"                  // This must match the ID inAddSecurityDefinition
                }
            },
            new string[] {} // Empty array means it's required for all actions
        }
    });
});

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.AllowAnyOrigin() // Allow requests from a specific origin (e.g., frontend app running on localhost:3000)
                   .AllowAnyHeader()  // Allow all headers (like Content-Type, Authorization)
                   .AllowAnyMethod(); // Allow all HTTP methods (GET, POST, PUT, DELETE)
        });
});


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

app.Run();
