using AppointmentSystem.Application.Interfaces;
using AppointmentSystem.Infrastructure.Data;
using AppointmentSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Controller and OpenAPI settings
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. Infrastructure: Add DbContext and Repository to the DI container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

var app = builder.Build();

// 3. Swagger and Middleware settings
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();