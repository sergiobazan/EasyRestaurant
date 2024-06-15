using Application;
using Carter;
using EasyRestaurant.API.Extentions;
using EasyRestaurant.API.Middlewares;
using EasyRestaurant.API.OptionsSetup;
using Infraestructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

builder.Services
    .AddApplication()
    .AddInfraestructure(builder.Configuration)
    .AddPresentation();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapCarter();

app.Run();