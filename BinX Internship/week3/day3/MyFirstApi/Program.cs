using MyFirstApi.Middleware;
using MyFirstApi.Services;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LibraryDatabase")));

builder.Services.AddScoped<IBookSummaryService, BookSummaryService>();

var app = builder.Build();


app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();