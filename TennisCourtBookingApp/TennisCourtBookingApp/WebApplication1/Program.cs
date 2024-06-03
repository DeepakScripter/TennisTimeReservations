using Microsoft.AspNetCore.Hosting;
using PdfSharp.Charting;
using TennisCourtBookingApp.Provider.IProvider;
using TennisCourtBookingApp.Provider.Provider;
using AutoMapper;
using TennisCourtBookingApp.Provider.Mapping;

    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ITennisCourtProvider, TennisCourtProvider>();
//builder.Services.AddScoped<ITennisCourtProvider, TennisCourtProvider>();
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
