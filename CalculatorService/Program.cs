using CalculatorDmain;
using CalculatorService.MiddleWare;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
IConfiguration config = builder.Configuration;

builder.Services.AddSingleton<ICalculatorDomain, CalculatorDomain>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Authorization")
    .AddScheme<AuthenticationSchemeOptions, AuthorizationHandler>("Authorization", null);

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
app.UseAuthorization();

app.Run();
