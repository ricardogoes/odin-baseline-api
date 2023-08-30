using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Odin.Baseline.Api.Configurations;
using Odin.Baseline.Api.Filters;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Infra.Messaging.JsonPolicies;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings
{
    AWSCognitoSettings = new AWSCognitoSettings
    {
        CognitoAuthorityUrl = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoAuthorityUrl")
    },
    ConnectionStrings = new ConnectionStrings
    {
        OdinBaselineDB = Environment.GetEnvironmentVariable("OdinSettings__ConnectionStrings__OdinBaselineDB")
    },
    CancellationTokenTimeout = builder.Configuration.GetSection("CancellationTokenTimeout").Get<int>()
};

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(typeof(ApiExceptionFilter));
    })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCasePolicy();
        jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddAppConnections(appSettings);

builder.Services.AddApplications();

builder.Services.AddRepositories();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddCognitoIdentity();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = appSettings.AWSCognitoSettings.CognitoAuthorityUrl;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = false
        };
});

builder.Services.AddApiVersioning(options =>
{
    // Retorna os headers "api-supported-versions" e "api-deprecated-versions"
    // indicando versoes suportadas pela API e o que esta como deprecated
    options.ReportApiVersions = true;

    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseResponseCompression();
app.MapControllers();

app.Run();

public partial class Program { }