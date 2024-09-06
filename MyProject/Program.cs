using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Reflection;
using MyProject.DataAccess;
using MyProject.Implementation.Validators;
using MyProject.Api.Core;
using MyProject.Application.Actor;
using MyProject.Implementation;
using MyProject.Api.Extensions;
using MyProject.Implementation.UseCases;
using MyProject.Application.Email;
using MyProject.Api.Core.FakeActor;
using MyProject.Api.Handlers.ProductOperations;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Api.Handlers.CartHandler;
using MyProject.Domain.Entities;
using MyProject.Implementation.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MyProject.Api.Core.Token;
using Projekat.Api.Core;
using ProjekatImplementation;
using MyProject.Api.Handlers.PaymentHandler;
using MyProject.Api.Handlers.ProducerHandler;
using MyProject.Api.Handlers.ProductRestaurantHandler;
using MyProject.Api.Handlers.RestaurantHandler;
using MyProject.Api.Handlers.StatusHandler;
using MyProject.Api.Handlers.UserHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjekatAsp", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName ?? type.Name);
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// MyDBContext
builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// validators
builder.Services.AddTransient<CreateProducerValidator>();
builder.Services.AddTransient<CreateCategoryValidator>();
builder.Services.AddTransient<CreateStatusValidator>();
builder.Services.AddTransient<CreateProductValidator>();
builder.Services.AddTransient<CreateRestaurantValidator>();
builder.Services.AddTransient<CreateProductRestaurantValidator>();
builder.Services.AddTransient<CreatePaymentValidator>();
builder.Services.AddTransient<CreateUserValidator>();
builder.Services.AddTransient<CreateCartValidator>();
builder.Services.AddTransient<CreateOrderValidator>();


// handlers
builder.Services.AddTransient<CreateProductHandler>();
builder.Services.AddTransient<UpdateProductHandler>();
builder.Services.AddTransient<GetAllProductsHandler>();
builder.Services.AddTransient<GetProductByIdHandler>();
builder.Services.AddTransient<DeleteProductHandler>();

builder.Services.AddTransient<CreateCategoryHandler>();
builder.Services.AddTransient<UpdateCategoryHandler>();
builder.Services.AddTransient<DeleteCategoryHandler>();
builder.Services.AddTransient<GetAllCategoriesHandler>();
builder.Services.AddTransient<GetCategoryByIdHandler>();

builder.Services.AddTransient<CreateCartHandler>();
builder.Services.AddTransient<UpdateCartHandler>();
builder.Services.AddTransient<DeleteCartHandler>();
builder.Services.AddTransient<GetAllCartItemsHandler>();

builder.Services.AddTransient<CreateOrderHandler>();

builder.Services.AddTransient<CreatePaymentHandler>();
builder.Services.AddTransient<UpdatePaymentHandler>();
builder.Services.AddTransient<DeletePaymentHandler>();
builder.Services.AddTransient<GetAllPaymentsHandler>();
builder.Services.AddTransient<GetPaymentByIdHandler>();

builder.Services.AddTransient<CreateProducerHandler>();
builder.Services.AddTransient<UpdateProducerHandler>();
builder.Services.AddTransient<DeleteProducerHandler>();
builder.Services.AddTransient<GetAllProducersHandler>();
builder.Services.AddTransient<GetProducerByIdHandler>();

builder.Services.AddTransient<CreatePRHandler>();
builder.Services.AddTransient<DeletePRHandler>();
builder.Services.AddTransient<GetAllPRHandler>();   

builder.Services.AddTransient<CreateRestaurantHandler>();
builder.Services.AddTransient<UpdateRestaurantHandler>();
builder.Services.AddTransient<DeleteRestaurantHandler>();
builder.Services.AddTransient<GetAllRestaurantsHandler>();
builder.Services.AddTransient<GetRestaurantByIdHandler>();

builder.Services.AddTransient<CreateStatusHandler>();
builder.Services.AddTransient<UpdateStatusHandler>();
builder.Services.AddTransient<DeleteStatusHandler>();
builder.Services.AddTransient<GetAllStatusesHandler>();
builder.Services.AddTransient<GetStatusByIdHandler>();


builder.Services.AddTransient<GetAllUsersHandler>();



// logger and audit services
builder.Services.AddTransient<AuditLog>();
builder.Services.AddTransient<AuditLogger>();

// token services
builder.Services.AddSingleton<ServerError>();
builder.Services.AddTransient<JwtTokenCreator>();
builder.Services.AddTransient<UseCaseHandler>();

// IUseCaseLogger implementation
builder.Services.AddTransient<IUseCaseLogger, DBUseCaseLogger>();

builder.Services.AddTransient<EfUserCreateCommand>();
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddTransient<UserService>();

// HTTP context and token storage
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ITokenStorage, InMemoryTokenStorage>();

// application actor provider and actor
builder.Services.AddTransient<IApplicationActorProvider>(x =>
{
    var accessor = x.GetService<IHttpContextAccessor>();
    var request = accessor.HttpContext?.Request;
    var authHeader = request?.Headers.Authorization.ToString();
    return new JwtApplicationActorProvider(authHeader);
});

builder.Services.AddTransient<IApplicationActor>(x =>
{
    var accessor = x.GetService<IHttpContextAccessor>();
    if (accessor.HttpContext == null)
    {
        return new UnauthorizedActor();
    }

    return x.GetService<IApplicationActorProvider>().GetActor();
});

// JWT 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "asp_api",
        ValidateIssuer = true,
        ValidAudience = "Any",
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsMyVerySecretKeyForAspProject")),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    cfg.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var tokenId = context.HttpContext.Request.GetTokenId();
            var storage = context.HttpContext.RequestServices.GetRequiredService<ITokenStorage>();

            if (!tokenId.HasValue || !storage.Exists(tokenId.Value))
            {
                context.Fail("Invalid token");
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                logger.LogWarning("Token validation failed. Token ID: {TokenId}", tokenId);
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
