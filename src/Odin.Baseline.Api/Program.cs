using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Odin.Baseline.Api;
using Odin.Baseline.Api.Attributes;
using Odin.Baseline.Api.IoC;
using Odin.Baseline.Crosscutting.AutoMapper;
using Odin.Baseline.Data.Persistence;
using Odin.Baseline.Domain.Models;

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

builder.Services.AddDbContext<OdinBaselineDbContext>(options => options.UseNpgsql(appSettings.ConnectionStrings.OdinBaselineDB));

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingConfiguration));

builder.Services.AddSingleton(appSettings);
builder.Services.AddScoped<ValidationFilterAttribute>();

ServiceBase.GetInstance<ServiceRepositories>().Add(builder.Services);
ServiceBase.GetInstance<ServiceServices>().Add(builder.Services);


builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true) ;
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddCognitoIdentity();
builder.Services.AddAuthentication(options =>
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
    // Agrupar por numero de versao
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // Necessario para o correto funcionamento das rotas
    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware(typeof(ErrorHandlingMiddleware));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();

app.Run();
