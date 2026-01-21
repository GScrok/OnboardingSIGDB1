using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Data;
using OnboardingSIGDB1.IOC;

var builder = WebApplication.CreateBuilder(args);

// <---- Infrastructure ----> 
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.ConfigureRepositories();
// <------------------------> 

// <---- Services ---->
builder.Services.ConfigureServices();
// <------------------> 

// <---- Mappers ---->
builder.Services.ConfigureMappers();
// <------------------> 

builder.Services.AddControllers();

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

app.Run();