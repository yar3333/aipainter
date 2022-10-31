call conda env create -f repo\environment.yml
call conda install -n invokeai -y scipy
conda install -n invokeai -y pytorch=1.12.1=py3.10_cuda11.6_cudnn8_0 -c pytorch
call conda install -n invokeai -y pyinstaller
