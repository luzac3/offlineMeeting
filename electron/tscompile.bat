
IF %1==Debug (
	call npm run build
)ELSE IF %1==Release (
	call npm run release
	call npx electron-forge import
	call npm run make
)