using Discount.API.Extensions;
using Discount.API.Repositories;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
var services= builder.Services;

builder.Services.AddControllers();
services.AddScoped<IDiscountRepository, DiscountRepository>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Discount.API", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Discount.API v1"));
}
app.UseAuthorization();

app.MapControllers();
app.MigrateDatabase<Program>();
app.Run();