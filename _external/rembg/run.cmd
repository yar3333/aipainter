@pushd repo

call conda run -n rembg --no-capture-output python rembg.py %*

@popd
