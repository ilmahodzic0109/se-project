using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SunglassesDAL.Model;
using Sunglasses.Services.Implementations;
using Sunglasses.Services.Interfaces;
using SunglassesDAL.Implementations;
using SunglassesDAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// Register DbContext with SQL Server connection string
builder.Services.AddDbContext<WebshopContext>(options =>
    options.UseSqlServer("Data Source=API2497;Initial Catalog=webshop;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"));


builder.Services.AddControllers();

builder.Services.AddScoped<IProducts, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAllOrigins");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
