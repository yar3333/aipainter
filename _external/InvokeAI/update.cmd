@pushd repo

git pull
call conda env update -n invokeai --file environment.yml --prune

@popd
