@pushd dist

set PATH=%~dp0..\_stuff\TensorRT-8.4.2.4\lib;%PATH%
set PATH=%~dp0..\_stuff\cudnn-windows-x86_64-8.4.1.50_cuda11.6\bin;%PATH%
aipainter_rembg\aipainter_rembg.exe s --port 9696

@popd
