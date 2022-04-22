using DemoProject.Api;
using DemoProject.ApplicationCore.Interfaces;
using DemoProject.ApplicationCore.Services;
using DemoProject.Infrastructure.Context;
using DemoProject.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

// register context
var connectionString = builder.Configuration["ConnectionStrings:DemoDb"];
builder.Services.AddDbContext<DemoContext>(options => options.UseSqlServer(connectionString));

// register services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// register repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthorization();
Middlewares(app, app.Environment);
app.MapControllers();

app.MapHealthChecks("/heartbeat");

app.Run();

void Middlewares(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.ConfigureExceptionHandler(env);
}