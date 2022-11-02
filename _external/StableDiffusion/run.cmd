@set PYTHON=
@set GIT=
@set VENV_DIR=
@set COMMANDLINE_ARGS=

@pushd repo
@call webui.bat --api --ckpt=%~dp0..\..\_stable_diffusion_checkpoints\sd-v1-4.ckpt %*
@popd
