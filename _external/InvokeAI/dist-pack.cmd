@rmdir /s /q dist

call conda run -n invokeai --cwd repo --no-capture-output pyinstaller ^
	--distpath ..\dist ^
	--collect-submodules torch ^
	--hidden-import=pytorch --collect-data torch --copy-metadata torch ^
	--hidden-import=scipy --collect-data scipy --copy-metadata scipy ^
	--hidden-import=certifi --collect-data certifi --copy-metadata certifi ^
	--hidden-import=huggingface_hub.hf_api ^
	--hidden-import=huggingface_hub.repository ^
	--hidden-import=k_diffusion --copy-metadata k_diffusion ^
	--paths="c:\WinProg\anaconda3\envs\invokeai\Lib\site-packages\wandb\vendor\gql-0.2.0" ^
	--hidden-import=wandb_gql ^
	--paths="c:\WinProg\anaconda3\envs\invokeai\Lib\site-packages\wandb\vendor\graphql-core-1.1" ^
	--hidden-import=wandb_graphql ^
	--collect-submodules dns ^
	--copy-metadata tqdm ^
	--copy-metadata regex ^
	--copy-metadata requests ^
	--copy-metadata packaging ^
	--copy-metadata filelock ^
	--copy-metadata numpy ^
	--copy-metadata tokenizers ^
	--copy-metadata rich ^
	--collect-submodules eventlet ^
	--collect-submodules eventlet.hubs ^
	--hidden-import=ldm.models.diffusion ^
    --hidden-import=ldm.models.diffusion.ddpm ^
	--hidden-import=ldm.modules.diffusionmodules.openaimodel ^
	--hidden-import=ldm.modules.encoders ^
	--hidden-import=ldm.modules.encoders.modules ^
	--hidden-import=ldm.modules.embedding_manager ^
	--add-binary "c:\WinProg\anaconda3\envs\invokeai\Lib\site-packages\pywin32_system32\pythoncom310.dll;." ^
	--paths="." ^
	--paths="c:\WinProg\anaconda3\envs\invokeai\Lib\site-packages\cv2" ^
	--paths="src\k-diffusion" ^
	--hidden-import=engineio.async_drivers.aiohttp ^
	--hidden-import=engineio.async_drivers.asgi ^
	--hidden-import=engineio.async_drivers.eventlet ^
	--hidden-import=engineio.async_drivers.gevent ^
	--hidden-import=engineio.async_drivers.gevent_uwsgi ^
	--hidden-import=engineio.async_drivers.sanic ^
	--hidden-import=engineio.async_drivers.threading ^
	--hidden-import=engineio.async_drivers.tornado ^
	--paths="src\gfpgan" ^
	--hidden-import=gfpgan  ^
	--hidden-import=gfpgan.archs  ^
	scripts/invoke.py

@if ERRORLEVEL 1 exit/b 1

mklink /d dist\models ..\repo\models
xcopy /e /q /i repo\configs dist\configs
echo D | xcopy /i repo\src\clip\clip\bpe_simple_vocab_16e6.txt.gz dist\invoke\clip

xcopy "c:\WinProg\anaconda3\envs\invokeai\Library\bin\uv.dll" dist\invoke\torch\lib
xcopy "c:\Program Files\NVIDIA Corporation\NvToolsExt\bin\x64\nvToolsExt64_1.dll" dist\invoke\torch\lib
xcopy /e /q /i c:\WinProg\anaconda3\envs\invokeai\Lib\site-packages\jsonschema\schemas dist\invoke\jsonschema\schemas
::xcopy /e /q /i c:\WinProg\anaconda3\envs\invokeai\Lib\site-packages\scipy.libs dist\invoke\scipy.libs

ren dist\invoke\invoke.exe aipainter_invokeai.exe
mklink /D dist\stuff ..\stuff

mkdir dist\ldm\invoke\restoration\codeformer\weights
mklink dist\ldm\invoke\restoration\codeformer\weights\codeformer.pth %~dp0repo\ldm\invoke\restoration\codeformer\weights\codeformer.pth

mkdir dist\src\gfpgan\experiments\pretrained_models
mklink dist\src\gfpgan\experiments\pretrained_models\GFPGANv1.4.pth %~dp0repo\src\gfpgan\experiments\pretrained_models\GFPGANv1.4.pth

call dist-run.cmd
