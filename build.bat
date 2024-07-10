@echo off

:: Check if dotnet is available
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo dotnet command could not be found. Please install .NET SDK.
    exit /b 1
)

:: Find the solution file
for %%f in (AutoAlteration.sln) do (
    set "solution_file=%%f"
    goto :found
)

echo No AutoAlteration.sln file found in the current directory.
exit /b 1

:found
:: Build the solution
dotnet build "%solution_file%"
