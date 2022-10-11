@pushd repo

@rmdir /s /q ..\dist

call conda run -n rembg --no-capture-output pyinstaller ^
	--distpath ..\dist ^
	--copy-metadata rembg ^
	rembg.py

@if ERRORLEVEL 1 (
	popd
	exit/b 1
)

@popd

call dist-run.cmd
