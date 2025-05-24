@description('The name of the Azure SQL server')
param sqlServerName string

@description('The administrator username for the Azure SQL server')
param adminUsername string

@description('The administrator password for the Azure SQL server')
@secure()
param adminPassword string

@description('The name of the Azure SQL database')
param sqlDatabaseName string

@description('The location for the Azure SQL resources')
param location string = resourceGroup().location
param tags object = {
  tagName: 'az204learning'
}

resource sqlServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: adminUsername
    administratorLoginPassword: adminPassword
  }
  sku: {
    name: 'GP_S_Gen5_2'
    tier: 'GeneralPurpose'
    capacity: 2
    family: 'Gen5'
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2022-02-01-preview' = {
  name: '${sqlServer.name}/${sqlDatabaseName}'
  location: location
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 2147483648
  }
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
}

output sqlServerName string = sqlServer.name
output sqlDatabaseName string = sqlDatabase.name
