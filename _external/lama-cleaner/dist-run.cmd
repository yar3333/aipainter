@pushd dist
@setlocal

set TORCH_HOME=%~dp0dist\stuff\models
main\aipainter_lamacleaner.exe --model=lama --device=cpu --port=9595

@endlocal
@popd
