#addin "Cake.Kudu"
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
    /*Information("Cleaning {0}", deploymentPath);
    CleanDirectories(deploymentPath.FullPath);

    Information("Deploying web to {0}", deploymentPath);
    CopyDirectory(websitePath, deploymentPath);*/
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
var envVars = Context.Environment.GetEnvironmentVariables();
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