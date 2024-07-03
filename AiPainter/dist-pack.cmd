call dist-build.cmd
@if ERRORLEVEL 1 exit /b 1

@pushd dist

@if EXIST aipainter.zip DEL /F aipainter.zip

7z a ^
	-x@..\dist-pack.exclude.txt ^
	-xr!.gitignore ^
	-mm=BZip2 -mx=1 -mcu ^
	aipainter.zip AiPainter

@popd
