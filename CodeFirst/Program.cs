using CodeFirst.DAL;
using CodeFirst.Repositories;
using CodeFirst.Repositories.Inetrfaces;
using CodeFirst.Services;
using CodeFirst.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HospitalContext>(opt =>
    opt.UseSqlServer(connectionString)
);
builder.Services.AddScoped<IPresciptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();


builder.Services.AddControllers();



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
