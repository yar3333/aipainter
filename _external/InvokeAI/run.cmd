::call conda run -n invokeai --cwd repo --no-capture-output python scripts/legacy_api.py --web %*

@setlocal
set PATH=%~dp0..\_stuff\NVIDIA GPU Computing Toolkit_CUDA_v11.4\bin;%PATH%
set TORCH_HOME=%~dp0stuff\models
set XDG_CACHE_HOME=%~dp0stuff\models
call conda run -n invokeai --cwd repo --no-capture-output python scripts/invoke.py --web %*
@endlocal
