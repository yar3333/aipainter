@pushd repo

git pull
call conda update --name invokeai -f environment.yml --prune

@popd
