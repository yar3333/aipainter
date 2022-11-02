@set PYTHON=
@set GIT=
@set VENV_DIR=
@set COMMANDLINE_ARGS=

@pushd repo
@call webui.bat --api %*
@popd
