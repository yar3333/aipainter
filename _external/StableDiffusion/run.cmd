@pushd repo
@call run.bat --api --ckpt=%~dp0..\..\_stable_diffusion_checkpoints\sd-v1-5-pruned-emaonly.ckpt %*
@popd
