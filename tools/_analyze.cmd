@echo off

set _msbuildPath="C:\Program Files\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin"

%_msbuildPath%\msbuild "..\src\CommandLine.sln" /t:Build /p:Configuration=Debug /v:m /m

"..\src\CommandLine\bin\Debug\net461\roslynator" analyze "..\src\Roslynator.sln" ^
 --msbuild-path %_msbuildPath% ^
 --analyzer-assemblies "..\src\CommandLine\bin\Debug\net461\Roslynator.CSharp.Analyzers.dll" ^
 --ignore-analyzer-references ^
 --ignored-diagnostics CS1591 ^
 --severity-level info ^
 --culture en ^
 --verbosity n ^
 --file-log "roslynator.log" ^
 --file-log-verbosity diag ^
 --output "diagnostics.xml"

pause
