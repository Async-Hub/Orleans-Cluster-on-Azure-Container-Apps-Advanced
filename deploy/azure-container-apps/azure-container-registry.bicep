@minLength(5)
@description('A unique suffix for names')
param appNamePrefix string ='shoppingapp'
param location string = resourceGroup().location

// Azure Container Registry
@description('Provide a globally unique name of your Azure Container Registry')
var acrName = 'acr${appNamePrefix}'
var tags = {
  Purpose: 'Azure Workshop'
}

// Azure Container Registry
resource acr 'Microsoft.ContainerRegistry/registries@2022-12-01' = {
  name: acrName
  location: location
  tags: tags
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

output acrName string = acrName
output acrUrl string = acr.properties.loginServer
