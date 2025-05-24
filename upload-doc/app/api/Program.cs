using StorageManagementAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AzureTableStorage.Extensions;
using StorageManagementAPI.Services;
using Microsoft.Extensions.Configuration;
using AzureBlobStorage.Extensions;
using storageapi.Infra.efcore;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
