@pushd repo

call conda create --name lama python=3.7.4
call conda run -n lama --no-capture-output pip install -r requirements.txt

@popd
