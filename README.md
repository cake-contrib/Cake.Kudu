# Cake Kudu
Cake add-in to help with Kudu deployments using C#
This is still early experimental, but feedback is welcome.

## Kudu
[Kudu](https://github.com/projectkudu/kudu) is the engine behind git deployments in Azure Web Sites. It can also run outside of Azure.

## Cake
[Cake](http://cakebuild.net/) (C# Make) is a cross platform build automation system.

## The Add-in
The Cake Kudu add-in parses the Kudu environment and exposed and easy accessible object model for your Cake C# build scripts.

This enables more advanced deployment scenarios like i.e. a single repository could contain multiple projects and based on the Azure web environment choose which project is built and deployed. An excellent use case for this is when you want to publish frontend and api web at the same time, just just commit once and both are deployed.

It will also enable to adjust build process/output based on configuration and not code.

## Features
The addin exposes 4 key areas of the Kudu environment

### Deployment
Exposes paths & tools used for deployment.

### SCM
Exposes source countrol properties like commit id, branch, etc.

### Tools
Exposed paths to tools that can be used in the Kudu environment. 

### WebSite
Exposed environmental information about running website, like hostname, compute mode, sku, etc.

### App Settings
Exposes web site app settings.

### Connection strings
Exposes web connection strings.

## Usage
Complete API/DSL reference can be found [here](http://cakebuild.net/dsl/kudu) 

The addin is loaded in Cake via the `#addin` directive
```csharp? 
#addin "Cake.Kudu"
```
This will give you a global `Kudu` property, to check if your running under `Kudu` or not you can use the `IsRunningOnKudu` property.
```csharp
if (Kudu.IsRunningOnKudu)
{
	//Deployment code
}
```

You can get a hold that the deployment target directory and check it's existens via the `Kudu.Deployment.Target` property.
```csharp
var deploymentPath = Kudu.Deployment.Target;
if (!DirectoryExists(deploymentPath))
{
    throw new DirectoryNotFoundException(
        string.Format(
            "Deployment target directory not found {0}",
            deploymentPath
            )
        );
}
```

The Kudu sync command is exposed via a convinent Sync method.
```csharp
Information("Deploying web from {0} to {1}", websitePath, deploymentPath);
Kudu.Sync(websitePath);
```
AppSettings can be accessed via the `IDictionary<string, string> AppSettings` property. 
```csharp
foreach(var appSetting in Kudu.AppSettings)
{
    Information(
        "Key: {0}Value: \"{1}\"",
        appSetting.Key.PadRight(40),
        appSetting.Value
        );
}
```
Connection strings can be accessed `IDictionary<string, string> AppSettings` property. 
```csharp
foreach(var conectionString in Kudu.ConnectionStrings)
{
    Information(
        "Key: {0}Value: \"{1}\"",
        conectionString.Key.PadRight(40),
        conectionString.Value
        );
}
```
An complete deployment script can be found [here](https://github.com/WCOMAB/Cake.Kudu/blob/master/deploy.cake)