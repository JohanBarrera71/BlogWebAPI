using System.Reflection;
using System.Text;
using DemoLinkedIn.Server.Filters;
using DemoLinkedIn.Server.Inicializer;
using DemoLinkedIn.Server.Repositories.Contracts;
using DemoLinkedIn.Server.Repositories.Implementations;
using DemoLinkedInApi.Data;
using DemoLinkedInApi.Entities;
using DemoLinkedInApi.Helpers;
using DemoLinkedInApi.Repositories.Contracts;
using DemoLinkedInApi.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
    options.Filters.Add<ExtractAndValidateUserIdFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ? This is for the model state validation
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

// JWT configuration
var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>();
builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSection.Issuer,
        ValidAudience = jwtSection.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key!))
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("UserPolicy", o => { o.RequireAuthenticatedUser(); });

// Database and Identity configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
                         throw new InvalidOperationException("Your connection is not found.")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.Tokens.EmailConfirmationTokenProvider = "Email";
    }).AddDefaultTokenProviders()
    .AddTokenProvider<EmailTokenProvider<ApplicationUser>>("Email")
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager();

// Services
builder.Services.AddScoped<IUserAccount, UserAccountRepository>();
builder.Services.AddScoped<IFeed, FeedRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDbInicializer, DbInicializer>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:50411")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Blog App API",
        Description =
            "Esta aplicación backend, desarrollada con C# y ASP.NET Web API, es una plataforma de blogging que permite a los usuarios \nautenticarse utilizando JSON Web Tokens (JWT) y ASP.NET Identity. A través de esta autenticación basada en tokens, los \nusuarios pueden gestionar sus cuentas, crear publicaciones y editar sus perfiles, accediendo a los servicios básicos \npara la administración de contenido y cuentas dentro de la plataforma.",
        Contact = new OpenApiContact
        {
            Email = "johan.dev.71@gmail.com",
            Name = "Diego Johan Lopez Barrera",
            Url = new Uri("https://johanbarrera71.github.io/porfolio.johan.dev/")
        },
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.EnableAnnotations();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog App API"); });

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var inicializer = services.GetRequiredService<IDbInicializer>();
        inicializer.Initialize();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.MapControllers();

app.Run();