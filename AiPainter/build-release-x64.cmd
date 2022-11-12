if exist release-x64 rmdir /s /q release-x64

dotnet publish -p:PublishSingleFile=true /p:DebugType=None -r win-x64 -c Release --self-contained false -o release-x64

if not exist release-x64\external                 mkdir release-x64\external
if not exist release-x64\external\_stuff          mklink /D release-x64\external\_stuff %~dp0..\_external\_stuff 2> nul
if not exist release-x64\external\StableDiffusion mklink /D release-x64\external\StableDiffusion %~dp0..\_external\StableDiffusion\repo 2> nul
if not exist release-x64\external\lama-cleaner    mklink /D release-x64\external\lama-cleaner %~dp0..\_external\lama-cleaner\dist 2> nul
if not exist release-x64\external\rembg           mklink /D release-x64\external\rembg %~dp0..\_external\rembg\dist 2> nul

if not exist release-x64\stable_diffusion_checkpoints mklink /D release-x64\stable_diffusion_checkpoints %~dp0..\_stable_diffusion_checkpoints 2> nul
