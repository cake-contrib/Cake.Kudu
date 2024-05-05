#load nuget:?package=Cake.Recipe&version=3.1.1

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            solutionFilePath: "./src/Cake.Kudu.sln",
                            title: "Cake.Kudu",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.Kudu",
                            appVeyorAccountName: "cakecontrib",
                            shouldRunInspectCode: false);

BuildParameters.PrintParameters(Context);

// There are no tests to run
((CakeTask)BuildParameters.Tasks.InstallOpenCoverTask.Task).Actions.Clear();
((CakeTask)BuildParameters.Tasks.DotNetCoreTestTask.Task).Actions.Clear();
((CakeTask)BuildParameters.Tasks.GenerateLocalCoverageReportTask.Task).Actions.Clear();

Build.RunDotNetCore();
