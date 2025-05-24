param storageAccountName string = 'azlearningsa'
var storageAccountSku = 'Standard_LRS'
var storageAccountKind = 'StorageV2'
param location string = resourceGroup().location
param tags object = {
  tagName: 'az204learning'
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountSku
  }
  kind: storageAccountKind
  properties: {
    accessTier: 'Hot'
  }
  tags: tags
}
