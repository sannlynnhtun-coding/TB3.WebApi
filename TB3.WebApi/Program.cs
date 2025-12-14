using Microsoft.EntityFrameworkCore;
using TB3.Database.AppDbContextModels;
using TB3.WebApi.Services;
using TB3.WebApi.Services.ProductCategory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IProductAdoDotNetService, ProductAdoDotNetV2Service>();
builder.Services.AddScoped<IProductAdoDotNetService, ProductAdoDotNetService>();
builder.Services.AddScoped<IProductDapperService, ProductDapperService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Product Category Services
builder.Services.AddScoped<IProductCategoryDapperService, ProductCategoryDapperService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")!);
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
