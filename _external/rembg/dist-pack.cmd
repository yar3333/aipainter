@pushd repo

@rmdir /s /q ..\dist

call conda run -n rembg --no-capture-output pyinstaller ^
	--distpath ..\dist ^
	--additional-hooks-dir ..\extra-hooks ^
	--collect-data torch --copy-metadata torch ^
	--collect-data llvmlite ^
	--collect-data certifi ^
	--collect-data onnxruntime.capi ^
	--collect-submodules uvicorn ^
	--collect-submodules uvicorn.loops ^
	--collect-submodules uvicorn.protocols.http ^
	--collect-submodules anyio ^
	--paths="c:\WinProg\anaconda3\envs\rembg\Lib\site-packages\cv2" ^
	main.py

@if ERRORLEVEL 1 (
	popd
	exit/b 1
)

xcopy ..\zlibwapi.dll ..\dist\

@popd

call dist-run.cmd
