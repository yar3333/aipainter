@pushd dist

@setlocal
set PATH=c:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v11.4\bin;%PATH%
legacy_api\aipainter_invokeai.exe --web %*
@endlocal

@popd
