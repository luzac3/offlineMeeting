@echo off
setlocal EnableExtensions

set "ELECTRON_VERSION=38.4.0"
set "APP_NAME=offlineMeeting"

cd /d "%~dp0"

if not exist "node_modules" call npm install || goto :error
call npm run release || goto :error

set "DIST=%~dp0dist"
set "CACHE=%DIST%\cache"
set "ZIP_NAME=electron-v%ELECTRON_VERSION%-linux-x64.zip"
set "ZIP=%CACHE%\%ZIP_NAME%"
set "OUT=%DIST%\%APP_NAME%-linux-x64"

if not exist "%CACHE%" mkdir "%CACHE%"
if not exist "%ZIP%" curl.exe -L -o "%ZIP%" "https://github.com/electron/electron/releases/download/v%ELECTRON_VERSION%/%ZIP_NAME%" || goto :error

if exist "%OUT%" rmdir /s /q "%OUT%"
mkdir "%OUT%" || goto :error
tar -xf "%ZIP%" -C "%OUT%" || goto :error

robocopy "%~dp0." "%OUT%\resources\app" /E /XD dist node_modules .git /NFL /NDL /NJH /NJS >nul
if %ERRORLEVEL% GEQ 8 goto :error

mkdir "%OUT%\config" || goto :error
copy /y "appData\json\setting.json" "%OUT%\config\" >nul || goto :error
copy /y "appData\menu.html" "%OUT%\config\" >nul || goto :error

ren "%OUT%\electron" "%APP_NAME%-electron" || goto :error

echo.
echo Output: %OUT%
echo On Linux run:
echo   chmod +x %APP_NAME%-electron chrome-sandbox chrome_crashpad_handler
echo   ./%APP_NAME%-electron --no-sandbox
exit /b 0

:error
echo Build failed.
exit /b 1
