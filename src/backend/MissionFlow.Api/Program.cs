using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MissionFlow.Api.Filters;
using MissionFlow.Api.Middleware;
using MissionFlow.Api.Services;
using MissionFlow.Application;
using MissionFlow.Application.Common.Behaviors;
using MissionFlow.Infrastructure;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.readFrom.Configuration(context.Configuration).Enrich.FromLogContext().Enrich.WithMachineName().WriteTo.Console(outputTemplate:"String"));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddTransient(typeof(MediaTR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>);
builder.Services.AddTransient(typeof(MediaTR.IPipelineBehavior<,>), typeof(LoggingBehavior<,>);

var jwtSecret = builder.Configuration["Jwt:Secret"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(options => { options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer(options => { options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true, ValidIssuer = jwtIssuer, ValidAudience = jwtAudience, IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)), ClockSkew = TimeSpan.Zero }; });
bp.GetConnectionString("Database").[ItProvider);