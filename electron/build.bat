@echo off
setlocal EnableExtensions

pushd "%~dp0" || exit /b 1

if not exist "node_modules" (
	call npm install || goto :error
)
call npm run release || goto :error
call npm run make || goto :error

popd
exit /b 0

:error
echo Build failed.
popd
exit /b 1
