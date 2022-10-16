@pushd repo

@rmdir /s /q ..\dist

call conda run -n lama --no-capture-output pyinstaller ^
	--distpath ..\dist ^
	--hidden-import=pytorch --collect-data torch --copy-metadata torch ^
	--collect-submodules pydantic ^
	--copy-metadata tqdm ^
	--copy-metadata regex ^
	--copy-metadata requests ^
	--copy-metadata packaging ^
	--copy-metadata filelock ^
	--copy-metadata numpy ^
	--copy-metadata tokenizers ^
	--hidden-import=jinja2.ext ^
	--paths="c:\WinProg\anaconda3\envs\lama\Lib\site-packages\cv2" ^
	main.py

@if ERRORLEVEL 1 (
	popd
	exit/b 1
)

ren ..\dist\main\main.exe aipainter_lamacleaner.exe 

@popd

call dist-run.cmd
