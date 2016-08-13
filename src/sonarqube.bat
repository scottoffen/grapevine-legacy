@ECHO OFF

SET ProjectKey="DM-1"
SET ProjectName="DeleteMe"
SET ProjectVersion="0.0.0.10"

SET TestProject=Grapevine.Tests
SET XunitReport=XUnitResults.xml
SET CoverageReport=OpenCoverResults.xml

SET SearchDirectory=%~dp0%TestProject%\bin\Debug
SET DllContainingTests="%~dp0%TestProject%\bin\Debug\%TestProject%.dll"

for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="xunit.console.exe" SET TestRunnerExe=%%~dpnxa
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="OpenCover.Console.exe" SET OpenCoverExe=%%~dpnxa

REM SonarQube Runner Begin
MSBuild.SonarQube.Runner.exe begin /k:%ProjectKey% /n:%ProjectName% /v:%ProjectVersion% /d:sonar.cs.opencover.reportsPaths="%~dp0%CoverageReport%"
rem /d:sonar.cs.xunit.reportsPaths="%~dp0%XunitReport%"
REM Run MSBuild
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /t:Rebuild

REM Run XUnit Tests
rem packages\xunit.runner.console.2.1.0\tools\xunit.console.exe %TestProject%\bin\Debug\%TestProject%.dll -xml %XunitReport%

REM Run OpenCover
"%OpenCoverExe%" ^
 -target:"%TestRunnerExe%" ^
 -targetargs:%DllContainingTests% ^
 -filter:"+[*]* -[*.Tests*]* -[*]*.*Config -[xunit*]*" ^
 -mergebyhash ^
 -skipautoprops ^
 -register:user ^
 -output:"%~dp0%CoverageReport%"^
 -searchdirs:"%SearchDirectory%"

REM SonarQube Runner End
MSBuild.SonarQube.Runner.exe end