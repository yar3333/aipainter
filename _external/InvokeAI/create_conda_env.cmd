@pushd repo

call conda env create -f environment.yaml
call conda run -n ldm --no-capture-output pip install -r requirements-win.txt

@popd
