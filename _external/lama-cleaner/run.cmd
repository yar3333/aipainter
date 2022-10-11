@pushd repo

call conda run -n lama --no-capture-output python main.py --model=lama --device=cpu --port=9595

@popd
