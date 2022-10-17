call conda create --name lama python=3.7.4
call conda run -n lama --cwd repo --no-capture-output pip install -r requirements.txt
call conda install -n lama -y pyinstaller
