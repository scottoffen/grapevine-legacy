@ECHO OFF

SET SearchDirectory=%~dp0Grapevine.Tests\bin\Debug
SET DllContainingTests=%~dp0Grapevine.Tests\bin\Debug\Grapevine.Tests.dll

rem ECHO %SearchDirectory%
rem ECHO %DllContainingTests%

for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="xunit.console.exe" SET TestRunnerExe=%%~dpnxa
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="OpenCover.Console.exe" SET OpenCoverExe=%%~dpnxa
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="ReportGenerator.exe" SET ReportGeneratorExe=%%~dpnxa

rem ECHO %TestRunnerExe%
rem ECHO %OpenCoverExe%
rem ECHO %ReportGeneratorExe%

if not exist "%~dp0GeneratedReports" mkdir "%~dp0GeneratedReports"
call :RunOpenCoverUnitTestMetrics

if %errorlevel% equ 0 (
 call :RunReportGeneratorOutput
)

if %errorlevel% equ 0 (
 call :RunLaunchReport
)
exit /b %errorlevel%

:RunOpenCoverUnitTestMetrics
"%OpenCoverExe%" ^
 -target:"%TestRunnerExe%" ^
 -targetargs:"\"%DllContainingTests%\"" ^
 -filter:"+[*]* -[*.Tests*]* -[*]*.*Config -[xunit*]* -[*]Grapevine.Interfaces.*" ^
 -mergebyhash ^
 -skipautoprops ^
 -register:user ^
 -output:"%~dp0GeneratedReports\CoverageReport.xml"^
 -searchdirs:"%SearchDirectory%"
exit /b %errorlevel%

:RunReportGeneratorOutput
"%ReportGeneratorExe%" ^
 -reports:"%~dp0\GeneratedReports\CoverageReport.xml" ^
 -targetdir:"%~dp0\GeneratedReports\ReportGeneratorOutput"
exit /b %errorlevel%

:RunLaunchReport
start "report" "%~dp0\GeneratedReports\ReportGeneratorOutput\index.htm"
exit /b %errorlevel%