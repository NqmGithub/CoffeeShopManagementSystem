﻿using CoffeeShopManagement.Models.Models;
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
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using CoffeeShopManagement.Business.VNPay.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CoffeeShopDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IProblemTypeRepository, ProblemTypeRepository>();
builder.Services.AddScoped<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IProblemTypeService, ProblemTypeService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CartCookieService>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
*/
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
//VNPay
builder.Services.AddScoped<IVnPayService, VnPayService>();

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

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");

app.UseStaticFiles(new StaticFileOptions()
{
   FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
 RequestPath = new PathString("/wwwroot")
});app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

