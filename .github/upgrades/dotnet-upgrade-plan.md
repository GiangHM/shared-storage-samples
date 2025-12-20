# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade storageapi.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                           | Current Version | New Version | Description                                                |
|:-------------------------------------------------------|:---------------:|:-----------:|:-----------------------------------------------------------|
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore   | 8.0.13          | 10.0.1      | Recommended for .NET 10.0                                  |
| Microsoft.AspNetCore.OpenApi                           |                 | 10.0.1      | Microsoft's official OpenAPI support                       |
| Microsoft.EntityFrameworkCore                          | 8.0.13          | 10.0.1      | Recommended for .NET 10.0                                  |
| Microsoft.EntityFrameworkCore.Relational               | 8.0.13          | 10.0.1      | Recommended for .NET 10.0                                  |
| Microsoft.EntityFrameworkCore.SqlServer                | 8.0.13          | 10.0.1      | Recommended for .NET 10.0                                  |
| Microsoft.Extensions.Logging.ApplicationInsights       | 2.22.0          |             | Remove and replace with OpenTelemetry                      |
| Microsoft.Extensions.Logging.AzureAppServices          | 8.0.8           | 10.0.1      | Recommended for .NET 10.0                                  |
| OpenTelemetry.Exporter.Console                         |                 | 1.10.0      | OpenTelemetry console exporter                             |
| OpenTelemetry.Extensions.Hosting                       |                 | 1.10.0      | OpenTelemetry hosting extensions                           |
| OpenTelemetry.Instrumentation.AspNetCore               |                 | 1.10.0      | OpenTelemetry ASP.NET Core instrumentation                 |
| OpenTelemetry.Instrumentation.Http                     |                 | 1.10.0      | OpenTelemetry HTTP instrumentation                         |
| Scalar.AspNetCore                                      |                 | 1.2.53      | Modern API documentation UI                                |
| Swashbuckle.AspNetCore                                 | 6.4.0           |             | Remove and replace with Microsoft.AspNetCore.OpenApi       |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### storageapi.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore should be updated from `8.0.13` to `10.0.1` (*recommended for .NET 10.0*)
  - Microsoft.AspNetCore.OpenApi should be added at version `10.0.1` (*Microsoft's official OpenAPI support*)
  - Microsoft.EntityFrameworkCore should be updated from `8.0.13` to `10.0.1` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.Relational should be updated from `8.0.13` to `10.0.1` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.SqlServer should be updated from `8.0.13` to `10.0.1` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Logging.ApplicationInsights should be removed (*replaced with OpenTelemetry*)
  - Microsoft.Extensions.Logging.AzureAppServices should be updated from `8.0.8` to `10.0.1` (*recommended for .NET 10.0*)
  - OpenTelemetry.Exporter.Console should be added at version `1.10.0` (*OpenTelemetry console exporter*)
  - OpenTelemetry.Extensions.Hosting should be added at version `1.10.0` (*OpenTelemetry hosting extensions*)
  - OpenTelemetry.Instrumentation.AspNetCore should be added at version `1.10.0` (*OpenTelemetry ASP.NET Core instrumentation*)
  - OpenTelemetry.Instrumentation.Http should be added at version `1.10.0` (*OpenTelemetry HTTP instrumentation*)
  - Scalar.AspNetCore should be added at version `1.2.53` (*Modern API documentation UI*)
  - Swashbuckle.AspNetCore should be removed (*replaced with Microsoft.AspNetCore.OpenApi*)

Other changes:
  - Program.cs should be updated to configure OpenTelemetry services instead of Application Insights
  - Program.cs should be updated to use Microsoft.AspNetCore.OpenApi and Scalar instead of Swashbuckle
