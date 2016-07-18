.\NuGet.exe restore ..\src\Kip.sln

$msbuild = ls 'C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe'

. $msbuild ..\src\Kip.sln /t:rebuild /p:Configuration=Release

if (-not (Test-Path bin)) { mkdir bin }

cp ..\src\Kip\bin\Release\Kip.dll bin

.\NuGet.exe pack .\Kip.nuspec
