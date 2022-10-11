@pushd repo

git pull
call conda env update --name ldm -f environment.yaml --prune

@popd
