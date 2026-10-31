@echo off
setlocal EnableExtensions
cd /d "%~dp0"

IF "%~1"=="Debug" (
	call npm run build || exit /b 1
	call npx electron .
)ELSE IF "%~1"=="Release" (
	call "%~dp0build.bat"
)ELSE (
	echo Usage: %~nx0 Debug^|Release
	exit /b 1
)
exit /b %ERRORLEVEL%
