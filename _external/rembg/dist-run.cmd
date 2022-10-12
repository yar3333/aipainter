@pushd dist

set PATH=%~dp0TensorRT-8.4.2.4\lib;%PATH%
set PATH=%~dp0cudnn-windows-x86_64-8.4.1.50_cuda11.6\bin;%PATH%
main\main.exe s --port 9696

@popd
