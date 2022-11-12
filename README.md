# AiPainter

Digital AI painter. Features:
 
 * generate pictures by text description (generate via [InvokeAI / StableDiffusion](https://github.com/invoke-ai/InvokeAI));
 * regenerate specified areas of image (inpaint via [InvokeAI / StableDiffusion](https://github.com/invoke-ai/InvokeAI));
 * remove specified objects from pictures (inpaint via [lama-cleaner](https://github.com/Sanster/lama-cleaner));
 * remove background from pictures (via [rembg](https://github.com/danielgatis/rembg)).

![screenshot-01](screenshots/screenshot-01.jpg)
![screenshot-02](screenshots/screenshot-02.jpg)
![screenshot-03](screenshots/screenshot-03.jpg)
![screenshot-04](screenshots/screenshot-04.jpg)
![screenshot-05](screenshots/screenshot-05.jpg)
![screenshot-06](screenshots/screenshot-06.jpg)
![screenshot-07](screenshots/screenshot-07.jpg)


## Outpainting (animated gif)

![outpainting](screenshots/outpainting.gif)


## Requirements

 * Windows 8+ (x64)
 * NVIDIA video card (>4GB RAM, CUDA 11.4)
 * 16 GB disk space

Tested on NVIDIA 3060.
 

## Using

 * download precompiled AiPainter from [mega.nz](https://mega.nz/file/sl9mDTbT#NJoZsKVPIs8mAQYZ9k1GNEtTnskuN-habkudIaxlz-U)
 * or from torrent (magnet:?xt=urn:btih:1f5cf38e1d5d44e6923501abf172b99b2008eba6&dn=aipainter-1.2.0.zip)
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


## Donuts

 This project fully free and open-source. You can help the project by sending a [small donut via Tinkoff](https://www.tinkoff.ru/cf/1P754cLgSiB).
