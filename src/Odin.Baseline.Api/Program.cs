using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Odin.Baseline.Api.Configurations;
using Odin.Baseline.Api.Filters;
using Odin.Baseline.Api.Middlewares;
using Odin.Baseline.Domain.Models.AppSettings;
using Odin.Baseline.Infra.Messaging.JsonPolicies;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var connectionStrings = new ConnectionStringsSettings(Environment.GetEnvironmentVariable("OdinSettings:ConnectionStrings:OdinBaselineDB")!);

var keycloakSettings = builder.Configuration.GetSection("Keycloak").Get<KeycloakSettings>()!;
keycloakSettings.Credentials!.Secret = Environment.GetEnvironmentVariable("OdinSettings:Keycloak:Credentials:Secret")!;

var appSettings = new AppSettings(connectionStrings, keycloakSettings);

var keycloakOptions = builder.Configuration
    .GetSection(KeycloakAuthenticationOptions.Section)
    .Get<KeycloakAuthenticationOptions>()!;

// Add services to the container.
builder.Services
    .AddSingleton(appSettings)
    .AddSingleton(keycloakOptions)
    .AddHttpContextAccessor()
    .AddAppConnections(appSettings)
    .AddApplications()
    .AddRepositories()
    .AddSecurity(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Odin Baseline", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
    })
    .AddApiVersioning(options =>
    {
        // Retorna os headers "api-supported-versions" e "api-deprecated-versions"
        // indicando versoes suportadas pela API e o que esta como deprecated
        options.ReportApiVersions = true;

        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
    })
    .AddVersionedApiExplorer(options =>
    {
        // Agrupar por numero de versao
        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        options.GroupNameFormat = "'v'VVV";

        // Necessario para o correto funcionamento das rotas
        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;
    })
    .AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
    })
    .AddControllers(options =>
    {
        options.Filters.Add(typeof(ApiExceptionFilter));
    })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCasePolicy();
        jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<TenantMiddleware>();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCompression();
app.MapControllers();

app.Run();

public partial class Program { }