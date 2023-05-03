using Microsoft.EntityFrameworkCore;
using AirlineAPI.Data;
using AirlineAPI.Extensions;
using Shared;
using Serilog;
using AirlineAPI.MessageBus;
using AirlineAPI.Configs;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddDbContext<AirlineDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AirlineDBContext") 
    ?? throw new InvalidOperationException("Connection string 'AirlineDBContext' not found.")));

builder.Services.Configure<RabbitMQConfig>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
    });


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

// app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<AirlineDBContext>((context, services) =>
{

}).Run();
