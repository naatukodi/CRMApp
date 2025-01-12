param location string = resourceGroup().location
param appServiceName string
param appServicePlanName string
param cosmosDbAccountName string
param databaseName string
param containerName string

module appServicePlan './appservice.bicep' = {
  name: 'appServicePlan'
  params: {
    location: location
    appServicePlanName: appServicePlanName
  }
}

module appService './appservice.bicep' = {
  name: 'appService'
  params: {
    location: location
    appServicePlanName: appServicePlanName
    appServiceName: appServiceName
  }
}

module cosmosDb './cosmosdb.bicep' = {
  name: 'cosmosDb'
  params: {
    location: location
    cosmosDbAccountName: cosmosDbAccountName
    databaseName: databaseName
    containerName: containerName
  }
}
