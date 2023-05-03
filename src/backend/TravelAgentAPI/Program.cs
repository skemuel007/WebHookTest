using AirlineAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Serilog;
using TravelAgentAPI.Data;
using TravelAgentAPI.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddDbContext<TravelAgentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TravelAgentDbContext") ?? throw new InvalidOperationException("Connection string 'TravelAgentDbContext' not found.")));

builder.Services.Configure<RabbitMQConfig>(builder.Configuration.GetSection("RabbitMQ"));

// builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MigrateDatabase<TravelAgentDbContext>((context, services) =>
{

}).Run();
