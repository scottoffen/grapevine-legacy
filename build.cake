#tool "nuget:?package=xunit.runner.console&version=2.1.0"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"
#addin "Cake.SemVer"
#addin "nuget:?package=Cake.Sonar"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var projectName = Argument<string>("projectName");
var toolVersion = Argument<string>("toolVersion", "4.6");
var version = Argument<string>("projectVersion");
var projectKey = Argument<string>("projectKey");

var toolVersions = new Dictionary<string,MSBuildToolVersion>
{
	{"4.0", MSBuildToolVersion.NET40},
	{"4.5", MSBuildToolVersion.NET45},
	{"4.6", MSBuildToolVersion.NET46}
};

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var projectPath = "./src/" + projectName;
var buildDir = Directory(projectPath +"/bin") + Directory(configuration);
var buildDirectory = "./.build";
var buildOutputPath = buildDirectory + "/" + toolVersion;
var testOutputPath = buildOutputPath + "/test-results";
var xunitFilePath = testOutputPath + projectName + ".Tests.dll.xml";
var coverageOutputPath = testOutputPath + "/coverage";
var coverageOutputFile = testOutputPath + "/coverage/result.xml";
var coverageReportsOutputPath = coverageOutputPath + "/reports";
var assemblyInfoPath = string.Format("{0}/Properties/AssemblyInfo.cs",projectPath);

void RunXUnit(ICakeContext cake, string path) {
    cake.XUnit2(path, new XUnit2Settings {
        HtmlReport = true,
        XmlReport = true,
        ShadowCopy = false,
        OutputDirectory = testOutputPath
    });
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    if(DirectoryExists(buildOutputPath))
    {
        CleanDirectory(buildOutputPath);
    }
});

Task("Version")
    .IsDependentOn("Clean")
    .Does(() => 
{
    var assemblyInfo = ParseAssemblyInfo(assemblyInfoPath);

    var updatedAssemblyInfo = new AssemblyInfoSettings
    {
        Version = version,
        FileVersion = version,
        InformationalVersion = version,
        Company=assemblyInfo.Company,
        Title=assemblyInfo.Title,
        Description=assemblyInfo.Description,
        Configuration=assemblyInfo.Configuration,
        Product=assemblyInfo.Product,
        Copyright=assemblyInfo.Copyright,
        Trademark=assemblyInfo.Trademark,
        Guid=assemblyInfo.Guid,
        InternalsVisibleTo=assemblyInfo.InternalsVisibleTo,
    };

    CreateAssemblyInfo(assemblyInfoPath, updatedAssemblyInfo);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Version")
    .Does(() =>
{
    NuGetRestore(projectPath +".sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    SonarBegin(new SonarBeginSettings{
        Name=projectName,
        Key=projectKey,
        OpenCoverReportsPath=coverageOutputFile,
        XUnitReportsPath=xunitFilePath,
        Version=version
    });

	if(!toolVersions.ContainsKey(toolVersion)) {
		throw new Exception(string.Format(@"The following tool version: {0}, is not contained in the valid tool versions {1}.
			 To Fix this add the tool version in the build.cake script", toolVersion, string.Join(",",toolVersions.Keys)));
	}
	
	CreateDirectory(buildDirectory); 
    CreateDirectory(buildOutputPath);    
	 
	// Use MSBuild
    MSBuild(projectPath + ".sln", new MSBuildSettings {
        ToolVersion = toolVersions[toolVersion],
        Configuration = configuration
    });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var unitTestPath = "./**/bin/Release/*.Tests.dll";
    CreateDirectory(testOutputPath);
    CreateDirectory(coverageOutputPath);
    OpenCover( cakeContext => {
            RunXUnit(cakeContext, unitTestPath);
        }, 
        new FilePath(coverageOutputFile),
        new OpenCoverSettings()
            .WithFilter(string.Format("+[{0}]*", projectName))
            .WithFilter("-[*.Tests]*")
    );

    CreateDirectory(coverageReportsOutputPath);
    ReportGenerator(coverageOutputFile, coverageReportsOutputPath, new ReportGeneratorSettings {
        ReportTypes = new ReportGeneratorReportType[]{ReportGeneratorReportType.Badges, ReportGeneratorReportType.Html}
    });

    SonarEnd(new SonarEndSettings{});
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");
    
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);