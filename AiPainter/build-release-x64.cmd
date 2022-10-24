dotnet publish -p:PublishSingleFile=true /p:DebugType=None -r win-x64 -c Release --self-contained false -o release-x64

if not exist release-x64\external              mkdir release-x64\external
if not exist release-x64\external\_stuff        mkdir release-x64\_stuff
if not exist release-x64\external\InvokeAI     mklink /D release-x64\external\InvokeAI %~dp0..\_external\InvokeAI\dist 2> nul
if not exist release-x64\external\lama-cleaner mklink /D release-x64\external\lama-cleaner %~dp0..\_external\lama-cleaner\dist 2> nul
if not exist release-x64\external\rembg        mklink /D release-x64\external\rembg %~dp0..\_external\rembg\dist 2> nul
