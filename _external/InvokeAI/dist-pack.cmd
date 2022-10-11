@pushd repo

@rmdir /s /q ..\dist

call conda run -n ldm --no-capture-output pyinstaller ^
	--distpath ..\dist ^
	--collect-submodules torch ^
	--hidden-import=pytorch --collect-data torch --copy-metadata torch ^
	--hidden-import=huggingface_hub.hf_api ^
	--hidden-import=huggingface_hub.repository ^
	--copy-metadata tqdm ^
	--copy-metadata regex ^
	--copy-metadata requests ^
	--copy-metadata packaging ^
	--copy-metadata filelock ^
	--copy-metadata numpy ^
	--copy-metadata tokenizers ^
    --hidden-import=ldm.models.diffusion.ddpm ^
	--hidden-import=ldm.modules.diffusionmodules.openaimodel ^
	--hidden-import=ldm.modules.encoders ^
	--hidden-import=ldm.modules.encoders.modules ^
	--hidden-import=ldm.modules.embedding_manager ^
	scripts\dream.py

@if ERRORLEVEL 1 (
	popd
	exit/b 1
)

xcopy /e /q /i configs ..\dist\configs
echo D | xcopy /i src\clip\clip\bpe_simple_vocab_16e6.txt.gz ..\dist\dream\clip
mklink /d ..\dist\models ..\models

xcopy          c:\WinProg\anaconda3\envs\ldm\Library\bin\uv.dll ..\dist\dream\torch\lib
xcopy "c:\Program Files\NVIDIA Corporation\NvToolsExt\bin\x64\nvToolsExt64_1.dll" ..\dist\dream\torch\lib

xcopy /e /q /i c:\WinProg\anaconda3\envs\ldm\Lib\site-packages\jsonschema\schemas ..\dist\dream\jsonschema\schemas

@popd

call dist-run.cmd
