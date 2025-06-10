@echo off

FOR /F "tokens=* USEBACKQ" %%F IN (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) DO (
SET msbuild="%%F"
)
ECHO %msbuild%

@%msbuild% OpenHardwareMonitor.sln /t:restore /p:RestorePackagesConfig=true
@%msbuild% OpenHardwareMonitor.sln /t:Rebuild /p:DebugType=None /p:Configuration=Release

if errorlevel 1 goto error

goto exit
:error
pause
:exit
