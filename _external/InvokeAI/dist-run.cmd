@pushd dist

@setlocal
set PATH=c:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v11.4\bin;%PATH%
dream\dream.exe --web %*
@endlocal

@popd
