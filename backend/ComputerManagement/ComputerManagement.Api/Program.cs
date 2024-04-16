﻿using ComputerManagement.BO;
using ComputerManagement.BO.DTO;
using ComputerManagement.Common.Configs;
using ComputerManagement.Service;
using ComputerManagement.Service.Implement;
using ComputerManagerment.Repos.Implement;
using ComputerManagerment.Repos.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using AutoMapper;
using ComputerManagement.Data;
using ComputerManagement.BO.Lib.Implement;
using ComputerManagement.BO.Lib.Interface;
using ComputerManagement.Data.Seed;
using ComputerManagement.Api.Middlewares;
using ComputerManagement.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.MigrationsAssembly("ComputerManagement.Data");
    });
    options.EnableSensitiveDataLogging();
});
builder.Services.AddTransient<SeederData>();

// add di jwtconfig
builder.Services.Configure<JwtConfig>(options =>
{
    builder.Configuration.GetSection("Jwt").Bind(options);
});

// add authen
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            //tự cấp token
            ValidateIssuer = false,
            ValidateAudience = false,

            // ký token
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            ValidateLifetime = true,
        };

    });

// add auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// add di
builder.Services.AddScoped<ContextData>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserRepop, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IComputerRoomRepo, ComputerRoomRepo>();
builder.Services.AddScoped<IComputerRoomService, ComputerRoomService>();

builder.Services.AddScoped<IComputerRepo, ComputerRepo>();
builder.Services.AddScoped<IComputerService, ComputerService>();

builder.Services.AddScoped<IComputerHistoryRepo, ComputerHistoryRepo>();
builder.Services.AddScoped<IComputerHistoryService, ComputerHistoryService>();

// add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });

});

var app = builder.Build();
app.UseCors("CorsPolicy");
if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<SeederData>();
        service.Seed();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<AuthMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();