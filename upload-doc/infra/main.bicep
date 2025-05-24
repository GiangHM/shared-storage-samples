param location string = resourceGroup().location
param storageAccountName string = 'azlearningsa'

@description('The name of the environment. This must be dev, test, or prod.')
@allowed([
  'dev'
  'test'
  'prod'
])
param environmentName string = 'dev'

@description('The unique name of the solution. This is used to ensure that resource names are unique.')
@minLength(5)
@maxLength(30)
param solutionName string = '${uniqueString(resourceGroup().id)}'

var appServicePlanName = '${environmentName}-${solutionName}-plan'
var appServiceAppApiName = '${environmentName}-${solutionName}-api'
var vueStaticAppName = '${environmentName}-${solutionName}-app'

@description('The name and tier of the App Service plan SKU.')
param appServicePlanSku object = {
  name: 'F1'
  tier: 'Free'
}

var tags = {
  tagName: 'az204learning'
}

module storageAccount 'storageaccount.bicep' = {
  name: 'storageAccount'
  params: {
    storageAccountName: storageAccountName
    location: location
    tags: tags
  }
}

module appService 'appservices.bicep' = {
  name: 'appService'
  params: {
    location: location
    appServicePlanName: appServicePlanName
    appServiceAppName: appServiceAppApiName
    appServicePlanSkuName: appServicePlanSku.name
    vueStaticAppName: vueStaticAppName
    tags: tags
  }
 
}

var sqlServerName = '${environmentName}-${solutionName}-sql'
@secure()
@description('The administrator login username for the SQL server.')
param sqlServerAdministratorLogin string

@secure()
@description('The administrator login password for the SQL server.')
param sqlServerAdministratorPassword string

module sqlService 'sqlservice.bicep' = {
  name: 'sqlService'
  params: {
    location: location
    sqlServerName: sqlServerName
    adminUsername: sqlServerAdministratorLogin
    adminPassword: sqlServerAdministratorPassword
    sqlDatabaseName: 'azlearningdb'
  }
}


@description('The host name to use to access the website.')
output websiteHostName string = appService.outputs.appServiceAppHostName

