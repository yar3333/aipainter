# AiPainter

Digital AI painter. Features:
 
 * generate pictures by text description (generate via [StableDiffusion](https://github.com/AUTOMATIC1111/stable-diffusion-webui));
 * regenerate specified areas of image (inpaint via [StableDiffusion](https://github.com/AUTOMATIC1111/stable-diffusion-webui));
 * remove specified objects from pictures (inpaint via [lama-cleaner](https://github.com/Sanster/lama-cleaner)).

![screenshot-01](screenshots/screenshot-01.jpg)
![screenshot-02](screenshots/screenshot-02.jpg)
![screenshot-03](screenshots/screenshot-03.jpg)
![screenshot-04](screenshots/screenshot-04.jpg)
![screenshot-05](screenshots/screenshot-05.jpg)


## Outpainting (animated gif)

![outpainting](screenshots/outpainting.gif)


## Requirements

 * Windows 10+ (x64)
 * 16+ GB RAM
 * NVIDIA video card (4+ GB VRAM, CUDA 11.4)
 * 20 GB disk space

Tested on NVIDIA 3060.
 

## Using

 * download precompiled AiPainter-2.1.0 from [mega.nz](https://mega.nz/file/N8tAgSKI#zBNVdaroqCioAV0xrrSXG-OEIxC1dYQqGUqt1Nb2tqU)
 * unpack
 * run `AiPainter.exe`


## Troubleshooting

 * look into `logs` folder
 * check `Config.json`


## Contribution

 * install [Anaconda](https://docs.anaconda.com/anaconda/install/windows/)
 * install MS Visual Studio Community 2022
 * `git clone git@github.com:yar3333/ai-image-processing.git --recurse-submodules`
 * look into subfolders in `_external` and fix paths in `*.cmd` helpers to conda `envs` directory
 * run `create_conda_env.cmd` helpers to prepare conda environments
 * now you can use `run.cmd` helpers to run python projects
 * open `AiPainter.sln` in Visual Studio and build it
