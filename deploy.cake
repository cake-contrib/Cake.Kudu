#tool "KuduSync.NET" "https://www.nuget.org/api/v2/"
#addin "Cake.Kudu" "https://www.nuget.org/api/v2/"
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target          = Argument<string>("target", "Default");
var configuration   = Argument<string>("configuration", "Release");
///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var websitePath     = MakeAbsolute(Directory("./src/TestWebSite"));
var solutionPath    = MakeAbsolute(File("./src/TestWebSite.sln"));
if (!Kudu.IsRunningOnKudu)
{
    throw new Exception("Not running on Kudu");
}

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


///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    //Clean up any binaries
    Information("Cleaning {0}", websitePath);
    CleanDirectories(websitePath + "/bin");
});

Task("Restore")
    .Does(() =>
{
    // Restore all NuGet packages.
    Information("Restoring {0}...", solutionPath);
    NuGetRestore(solutionPath);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    // Build all solutions.
    Information("Building {0}", solutionPath);
    MSBuild(solutionPath, settings =>
        settings.SetPlatformTarget(PlatformTarget.MSIL)
            .WithProperty("TreatWarningsAsErrors","true")
            .WithTarget("Build")
            .SetConfiguration(configuration));
});


Task("Publish")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Deploying web from {0} to {1}", websitePath, deploymentPath);
    Kudu.Sync(websitePath);
});


Task("Default")
    .IsDependentOn("Publish");


///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);


///////////////////////////////////////////////////////////////////////////////
// TEMP DEBUG CODE
///////////////////////////////////////////////////////////////////////////////
Information("AppSettings");
foreach(var appSetting in Kudu.AppSettings)
{
    Information(
        "Key: {0}Value: \"{1}\"",
        appSetting.Key.PadRight(40),
        appSetting.Value
        );
}

Information("ConnectionStrings");
foreach(var conectionString in Kudu.ConnectionStrings)
{
    Information(
        "Key: {0}Value: \"{1}\"",
        conectionString.Key.PadRight(40),
        conectionString.Value
        );
}

Information("EnvironmentVariables");
var envVars = EnvironmentVariables();
string path;
if (envVars.TryGetValue("PATH", out path))
{
    Information("Path: {0}", path);
}
foreach(var envVar in envVars)
{
    Information(
        "Key: {0}Value: \"{1}\"",
        envVar.Key.PadRight(40),
        envVar.Value
        );
}

//var indexHtmlPath = deploymentPath.CombineWithFilePath("index.html");
//System.IO.File.WriteAllText(indexHtmlPath.ToString(), "Hello from Cake!", Encoding.UTF8);