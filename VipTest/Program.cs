using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using VipTest.Auth;
using VipTest.Localization;
using VipTest.Utlity;
using VipTest.Data;
using VipTest.Files;
using VipTest.Hubs;
using VipTest.RideBillings;

var builder = WebApplication.CreateBuilder(args);
// Add configuration
ConfigProvider.config = builder.Configuration;

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<ILocalizationService, LocalizationService>();

// Register the EnumTranslationService
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IEnumTranslationService, EnumTranslationService>();

// Register DbContext
builder.Services.AddDbContext<VipProjectContext>(options =>
        options.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
            .EnableSensitiveDataLogging()
            .UseNpgsql(builder.Configuration.GetConnectionString("local")).EnableSensitiveDataLogging(),
    ServiceLifetime.Scoped);


// Add application-specific services
builder.Services.AddApplicationServices(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder.WithOrigins(
                "http://localhost:63342", "http://localhost:3000",
                "http://localhost:3001", "http://localhost:3002",
                "http://localhost:3003", "http://localhost:3004",
                "http://localhost:3005", "http://localhost:3006",
                "http://192.168.1.8:3000/"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("status", "Content-Disposition",
                "Access-Control-Allow-Origin"));
});


// Add controllers and configure JSON serialization, localization, and view support
builder.Services.AddControllers(options =>
    {
        options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter
            { DateTimeStyles = DateTimeStyles.AssumeUniversal });
    })
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    })
    .AddViewLocalization();
    

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    options.OperationFilter<AddAcceptLanguageHeaderParameter>(); // Existing filter for Accept-Language
    // options.OperationFilter<AddJwtAuthorizationParameter>(); // Add the JWT authorization parameter conditionally

    // Add JWT Authentication to Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: 'Bearer 12345abcdef'",
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
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
            new string[] { }
        }
    };

    options.AddSecurityRequirement(securityRequirement);
});

// Register Swagger examples filter
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
builder.Services.AddSignalR();


// Build the application
var app = builder.Build();

// Localization options
var supportedCultures = new[] { "en", "ar" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

// Add Accept-Language header provider
localizationOptions.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());

// Middleware setup
app.UseCors("AllowLocalhost");
app.UseHttpsRedirection();
app.UseStaticFiles();

// Use localization middleware
app.UseRequestLocalization(localizationOptions);

// Ensure routing comes before authentication and authorization
app.UseRouting();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Enable Swagger and routing
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger";
    c.DocExpansion(DocExpansion.None);
});

// Mapping the controllers after routing, authentication, and authorization
app.MapControllers();

// Run the RideBillingTypesConfig seeder here
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<RideBillingTypesConfigSeeder>();
    await seeder.SeedAsync();
}

app.MapHub<TrackingHub>("/hubs/trackingHub");

// Run the application
app.Run();