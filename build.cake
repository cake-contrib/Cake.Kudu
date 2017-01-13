#addin "nuget:https://www.nuget.org/api/v2?package=Newtonsoft.Json&version=9.0.1"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target          = Argument<string>("target", "Default");
var configuration   = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var isLocalBuild        = !AppVeyor.IsRunningOnAppVeyor;
var isPullRequest       = AppVeyor.Environment.PullRequest.IsPullRequest;
var solutions           = GetFiles("./**/*.sln");
var solutionPaths       = solutions.Select(solution => solution.GetDirectory());
var releaseNotes        = ParseReleaseNotes("./ReleaseNotes.md");
var version             = releaseNotes.Version.ToString();
var binDir              = "./src/Cake.Kudu/bin/" + configuration;
var nugetRoot           = "./nuget/";
var versionSuffix       = isLocalBuild
                                ? string.Empty
                                : string.Concat("-build-", AppVeyor.Environment.Build.Number);
var semVersion          = string.Concat(version, versionSuffix);

var assemblyInfo        = new AssemblyInfoSettings {
                                Title                   = "Cake.Kudu",
                                Description             = "Cake Kudu AddIn",
                                Product                 = "Cake.Kudu",
                                Company                 = "WCOM AB",
                                Version                 = version,
                                FileVersion             = version,
                                InformationalVersion    = semVersion,
                                Copyright               = string.Format("Copyright © WCOM AB {0}", DateTime.Now.Year),
                                CLSCompliant            = true,
                                ComVisible              = false
                            };
var nuGetPackSettings   = new NuGetPackSettings {
                                Id                      = assemblyInfo.Product,
                                Version                 = assemblyInfo.InformationalVersion,
                                Title                   = assemblyInfo.Title,
                                Authors                 = new[] {assemblyInfo.Company},
                                Owners                  = new[] {assemblyInfo.Company},
                                Description             = assemblyInfo.Description,
                                Summary                 = "Cake AddIn that extends Cake with Kudu evironment and deployment features",
                                ProjectUrl              = new Uri("https://github.com/WCOMAB/Cake.Kudu/"),
                                IconUrl                 = new Uri("http://cdn.rawgit.com/WCOMAB/nugetpackages/master/Chocolatey/icons/wcom.png"),
                                LicenseUrl              = new Uri("https://github.com/WCOMAB/Cake.Kudu/blob/master/LICENSE"),
                                Copyright               = assemblyInfo.Copyright,
                                ReleaseNotes            = releaseNotes.Notes.ToArray(),
                                Tags                    = new [] {"Cake", "Script", "Build", "Kudu", "Azure", "Deployment"},
                                RequireLicenseAcceptance= false,
                                BasePath                = binDir,
                                OutputDirectory         = nugetRoot
                            };

if (!isLocalBuild)
{
    AppVeyor.UpdateBuildVersion(semVersion);
}

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");

    var buildStartMessage = string.Format(
                            "Building version {0} of {1} ({2}).",
                            version,
                            assemblyInfo.Product,
                            semVersion
                            );

    Information(buildStartMessage);
});

Teardown(context =>
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
    // Clean solution directories.
    foreach(var path in solutionPaths)
    {
        Information("Cleaning {0}", path);
        CleanDirectories(path + "/**/bin/" + configuration);
        CleanDirectories(path + "/**/obj/" + configuration);
    }
    Information("Cleaning {0}", nugetRoot);
    CleanDirectory(nugetRoot);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(ctx => {
        DotNetCoreRestore("./", new DotNetCoreRestoreSettings {
            Sources = new [] { "https://api.nuget.org/v3/index.json" },
            Verbosity = DotNetCoreRestoreVerbosity.Warning
        });
});

Task("Patch-Project-Json")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var projects = GetFiles("./src/**/project.json");
    foreach(var project in projects)
    {
        if(!PatchProjectJson(project, version, nuGetPackSettings)) {
            Warning("No version specified in {0}.", project.FullPath);
        }
    }
});

Task("SolutionInfo")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Patch-Project-Json")
    .Does(() =>
{
    var file = "./src/SolutionInfo.cs";
    CreateAssemblyInfo(file, assemblyInfo);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("SolutionInfo")
    .IsDependentOn("Patch-Project-Json")
    .Does(() =>
{
   var projects = GetFiles("./**/project.json");
    foreach(var project in projects)
    {
        DotNetCoreBuild(project.GetDirectory().FullPath, new DotNetCoreBuildSettings {
            Configuration = configuration
        });
    }
});


Task("Create-NuGet-Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    if (!DirectoryExists(nugetRoot))
    {
        CreateDirectory(nugetRoot);
    }

    var projects = GetFiles("./**/project.json");
    foreach(var project in projects)
    {
        DotNetCorePack(project.GetDirectory().FullPath, new DotNetCorePackSettings {
            VersionSuffix = versionSuffix.TrimStart('-'),
            Configuration = configuration,
            OutputDirectory = nugetRoot,
            NoBuild = true,
            Verbose = false
        });
    }
});

Task("Publish-MyGet")
    .IsDependentOn("Create-NuGet-Package")
    .WithCriteria(() => !isLocalBuild)
    .WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("MYGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }

    var source = EnvironmentVariable("MYGET_SOURCE");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve MyGet source.");
    }

    // Get the path to the package.
    var package = nugetRoot + "Cake.Kudu." + semVersion + ".nupkg";

    // Push the package.
    NuGetPush(package, new NuGetPushSettings {
        Source = source,
        ApiKey = apiKey
    });
});


Task("Default")
    .IsDependentOn("Create-NuGet-Package");

Task("AppVeyor")
    .IsDependentOn("Publish-MyGet");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);

public static bool PatchProjectJson(FilePath project, string version, NuGetPackSettings nuGetPackSettings)
{
    var content = System.IO.File.ReadAllText(project.FullPath, Encoding.UTF8);
    var node = Newtonsoft.Json.Linq.JObject.Parse(content);
    if(node["version"] != null)
    {
        node["version"].Replace(string.Concat(version, "-*"));
        node["description"]?.Replace(nuGetPackSettings.Description);
        node["copyright"]?.Replace(nuGetPackSettings.Copyright);
        node["title"]?.Replace(nuGetPackSettings.Title);
        node["packOptions"]?["summary"]?.Replace(nuGetPackSettings.Summary);
        node["packOptions"]?["projectUrl"]?.Replace(nuGetPackSettings.ProjectUrl);
        node["packOptions"]?["licenseUrl"]?.Replace(nuGetPackSettings.LicenseUrl);
        node["packOptions"]?["iconUrl"]?.Replace(nuGetPackSettings.IconUrl);
        node["packOptions"]?["releaseNotes"]?.Replace(string.Join("\r\n", nuGetPackSettings.ReleaseNotes));
        node["packOptions"]?["requireLicenseAcceptance"]?.Replace(nuGetPackSettings.RequireLicenseAcceptance);
        node["packOptions"]?["tags"]?.Replace(Newtonsoft.Json.Linq.JToken.FromObject(nuGetPackSettings.Tags));
        System.IO.File.WriteAllText(project.FullPath, node.ToString(), Encoding.UTF8);
        return true;
    };
    return false;
}
