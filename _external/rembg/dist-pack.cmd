@pushd repo

@rmdir /s /q ..\dist

echo F | xcopy /y rembg.py aipainter_rembg.py

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
	aipainter_rembg.py

@if ERRORLEVEL 1 (
	del aipainter_rembg.py
	del aipainter_rembg.spec
	popd
	exit/b 1
)

del aipainter_rembg.py
del aipainter_rembg.spec

@popd

xcopy stuff\zlibwapi.dll dist\aipainter_rembg\
mklink /D dist\stuff ..\stuff

call dist-run.cmd
