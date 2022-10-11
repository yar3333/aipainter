@pushd repo

call conda run -n ldm --no-capture-output python scripts/dream.py %*

@popd
