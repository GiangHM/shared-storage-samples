using StorageManagementAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AzureTableStorage.Extensions;
using StorageManagementAPI.Services;
using Microsoft.Extensions.Configuration;
using AzureBlobStorage.Extensions;
using storageapi.Infra.efcore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Configure OpenTelemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("StorageManagementAPI"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter());

builder.Services.AddAzureTableStorage();
builder.Services.AddTransient<IDocTypeTableService, DocTypeTableService>();
builder.Services.AddAzureBlobStorage();
builder.Services.AddSqlDatabaseServer(options => builder.Configuration.GetSection("Storage:SqlDb").Bind(options));

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AutoMapperProfile());
});

var frontUrl = builder.Configuration.GetValue<string>("FrontUrl");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(frontUrl)
                          .AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyHeader()
                          .SetIsOriginAllowed((host) => true);
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add OpenAPI support
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.MapScalarApiReference();
//}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
