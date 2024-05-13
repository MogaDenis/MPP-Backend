using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MPP_Backend.Business.Mappings;
using MPP_Backend.Business.Services;
using MPP_Backend.Business.Services.Interfaces;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories;
using MPP_Backend.Data.Repositories.Interfaces;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddDbContext<CarManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

builder.Services.AddAutoMapper(typeof(CarMappingProfile));
builder.Services.AddAutoMapper(typeof(OwnerMappingProfile));

builder.Services.AddSingleton<ICarRepository, CarRepository>();
builder.Services.AddSingleton<IOwnerRepository, OwnerRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddSingleton<ICarService, CarService>();
builder.Services.AddSingleton<IOwnerService, OwnerService>();
builder.Services.AddSingleton<IUserService, UserService>();

//builder.Services.AddScoped<ICarRepository, CarRepository>();
//builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();

//builder.Services.AddScoped<ICarService, CarService>();
//builder.Services.AddScoped<IOwnerService, OwnerService>();
//builder.Services.AddScoped<IUserService, UserService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("Location"));

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetService<CarManagerContext>();
        context?.Database.EnsureCreated();
        context?.Database.Migrate();
    }
    catch (Exception ex)
    {
        Debug.WriteLine(ex);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

