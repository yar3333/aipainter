@pushd repo

call conda create --name rembg python=3.9
call conda run -n rembg --no-capture-output pip install -r requirements.txt
call conda run -n rembg --no-capture-output pip install -r requirements-gpu.txt

@popd
