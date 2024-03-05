using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Text;
using UserApp.Helpers.DIHelper;
using UserApp.Mappers.MappersConfig;
using UserApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "RestrictDomain";

// Adding FluentValidation for ASP.NET Core
builder.Services.AddControllers().AddFluentValidation(v =>
{
    v.ImplicitlyValidateChildProperties = true;
    v.ImplicitlyValidateRootCollectionElements = true;
    v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

// Adding JWT Bearer Authentication and Swagger/OpenAPI documentation
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

// Setting up JWT authentication with validation parameters
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = "http://localhost:5043",
                ValidAudience = "http://localhost:5043",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My secret code must be very long"))
            };
        });

// Setting up CORS to accept requests only from the specified domain dummy.com
builder.Services.AddCors(
    policyBuilder => policyBuilder.AddPolicy(corsPolicy, policy => policy.WithOrigins("http://www.dummy.com/", "https://www.dummy.com/"))
);

// Adding connection string for database setup
builder.Services.InjectDbContext();

// Injecting dependencies for repositories and services
builder.Services.InjectRepositories();
builder.Services.InjectServices();

// Adding AutoMapper configuration
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// Setting up Serilog for logging
//builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.File("logs.txt")); - po ednostaven nacin za logging

Log.Logger = new LoggerConfiguration()
           .Enrich.FromLogContext()
           .MinimumLevel.Information()
           .WriteTo.File(
               $@"{AppDomain.CurrentDomain.BaseDirectory}Logs\User_LOG_{DateTime.Now.Date:dd-MM-yyyy}.txt",
               LogEventLevel.Information,
               "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
           .CreateLogger();



var app = builder.Build();

app.UseCors(corsPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
