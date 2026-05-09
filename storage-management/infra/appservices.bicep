@description('The Azure region into which the resources should be deployed.')
param location string

@description('The name of the App Service app.')
param appServiceAppName string

@description('The name of the App Service plan.')
param appServicePlanName string

@description('The name of the App Service plan SKU.')
param appServicePlanSkuName string

@description('The tags to be applied to the resources.')
param tags object = {}

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: appServicePlanSkuName
  }
  tags: tags
}

resource appServiceApp 'Microsoft.Web/sites@2024-04-01' = {
  name: appServiceAppName
  location: location
  tags: tags
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      netFrameworkVersion: 'v8.0' // Specifies .NET 8
      alwaysOn: true
    }
  }
}

// @description('The name of the App Service app for the Node.js Vue 3 application.')
// param nodeAppServiceAppName string

// resource nodeAppServiceApp 'Microsoft.Web/sites@2024-04-01' = {
//   name: nodeAppServiceAppName
//   location: location
//   tags: tags
//   properties: {
//     serverFarmId: appServicePlan.id
//     httpsOnly: true
//     siteConfig: {
//       linuxFxVersion: 'NODE|18-lts' // Specifies Node.js 18 LTS
//       appCommandLine: '' // Optional: Specify a custom start command if needed
//     }
//   }
// }

@description('The default host name of the App Service app.')
output appServiceAppHostName string = appServiceApp.properties.defaultHostName
// output nodeAppServiceApp string = nodeAppServiceApp.properties.defaultHostName
