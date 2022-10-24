call build-release-x64.cmd
@if ERRORLEVEL 1 exit /b 1

if exist dist rmdir /s /q dist
mkdir dist
echo D | mv release-x64 dist\AiPainter

@pushd dist

7z a ^
	-x@..\pack-to-dist.exclude.txt ^
	-xr!.gitignore ^
	-mm=BZip2 -mx=5 -mcu ^
	aipainter.zip AiPainter

@popd