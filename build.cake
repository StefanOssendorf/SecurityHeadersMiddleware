#addin "nuget:?package=NuGet.Core"
#addin "Cake.FileHelpers"
#addin "Cake.Incubator"
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=vswhere"

var target        = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildNumber   = Argument("buildnumber", "0");
var buildDir      = Directory("./artifacts");
var solution      = "./SecurityHeaders.sln";


Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("RestorePackages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
	DirectoryPath vsLatest  = VSWhereLatest();
	FilePath msBuildPathX64 = (vsLatest==null)
								? null
								: vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/amd64/MSBuild.exe");

	MSBuild(solution, new MSBuildSettings {
		ToolPath = msBuildPathX64,
		Configuration = configuration,
		Verbosity = Verbosity.Minimal
	});
	
    // MSBuild(solution, settings => settings
        // .SetConfiguration(configuration)
        // .SetVerbosity(Verbosity.Minimal)
        // .UseToolVersion(MSBuildToolVersion.VS2017)
    // );
});

Task("RunTests")
    .IsDependentOn("Build")
    .Does(() =>
{

	var projects = GetFiles("./src/**/*.Tests.csproj");
	
	foreach(var project in projects)
	{
		DotNetCoreTest(project.FullPath);
	}
});

Task("Default")
    .IsDependentOn("RunTests");

RunTarget(target);