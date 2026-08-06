using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaskFlowDbContext>(options =>
    options.UseSqlite("Data Source=taskflow.db"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
