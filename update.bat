@echo off

:: Check if git is available
where git >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo git command could not be found. Please install Git.
    exit /b 1
)

:: Check if the current directory is a git repository
if not exist ".git" (
    echo This directory is not a git repository.
    exit /b 1
)

:: Fetch updates from the remote
echo Fetching updates from the remote repository...
git fetch

:: Pull updates from the remote repository
echo Pulling updates from the remote repository...
git pull
