using Microsoft.EntityFrameworkCore;
using SignalR.Covid19Chart.API.Hubs;
using SignalR.Covid19Chart.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQL"));
});
builder.Services.AddSignalR();
builder.Services.AddScoped<CovidService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:7068").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHub<CovidHub>("/CovidHub");
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
