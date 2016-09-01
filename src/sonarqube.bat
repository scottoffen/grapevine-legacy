@ECHO OFF

if [%1]==[] goto :usage

SET ProjectKey="GV4"
SET ProjectName="Grapevine 4"
SET ProjectVersion="4.0.0.%1"

SET TestProject=Grapevine.Tests
SET XunitReport=%~dp0XUnitResults.xml
SET CoverageReport=%~dp0OpenCoverResults.xml

SET SearchDirectory=%~dp0%TestProject%\bin\Debug
SET DllContainingTests="%~dp0%TestProject%\bin\Debug\%TestProject%.dll"

for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="xunit.console.exe" SET TestRunnerExe=%%~dpnxa
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="OpenCover.Console.exe" SET OpenCoverExe=%%~dpnxa

REM SonarQube Runner Begin
MSBuild.SonarQube.Runner.exe begin /k:%ProjectKey% /n:%ProjectName% /v:%ProjectVersion% /d:sonar.cs.opencover.reportsPaths="%CoverageReport%" /d:sonar.cs.xunit.reportsPaths="%XunitReport%"

REM Run MSBuild
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /t:Rebuild

REM Run XUnit Tests
packages\xunit.runner.console.2.1.0\tools\xunit.console.exe %TestProject%\bin\Debug\%TestProject%.dll -xml %XunitReport%

REM Run OpenCover
"%OpenCoverExe%" ^
 -target:"%TestRunnerExe%" ^
 -targetargs:%DllContainingTests% ^
 -filter:"+[*]* -[*.Tests*]* -[*]*.*Config -[xunit*]*" ^
 -mergebyhash ^
 -skipautoprops ^
 -register:user ^
 -output:"%CoverageReport%"^
 -searchdirs:"%SearchDirectory%"

REM SonarQube Runner End
MSBuild.SonarQube.Runner.exe end

REM Clean Up Files
del %XunitReport%
del %CoverageReport%

goto :end

:usage
@echo Usage: %0 ^<BuildNumber^>
@echo The provided ^<BuildNumber^> will be appended to the end of the project version

goto :end

:end
exit /B