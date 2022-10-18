# ai-image-processing

A meta-repo for "AiPainter" application. Contains AiPainter source codes (C# / Forms) and related external subrepos (python).


## AiPainter

Digital painter. Features:
 
 * generate pictures by text description (generate via [InvokeAI / StableDiffusion](https://github.com/invoke-ai/InvokeAI));
 * regenerate specifed areas of image (inpaint via [InvokeAI / StableDiffusion](https://github.com/invoke-ai/InvokeAI));
 * remove specifed objects from pictures (inpaint via [lama-cleaner](https://github.com/Sanster/lama-cleaner));
 * remove background from pictures (via [rembg](https://github.com/danielgatis/rembg)).

![screenshot](screenshot.png)


## Requirements

 * NVIDIA video card (>4GB RAM);
 * video card must support CUDA 11.4.

Tested on NVIDIA 3060.
 

## Using

 * download zip, unpack;
 * download StableDuffision network weights (file `sd-v1-4.ckpt`) from [HuggingFace](https://huggingface.co/CompVis/stable-diffusion-v-1-4-original);
 * save `sd-v1-4.ckpt` as `external\InvokeAI\models\ldm\stable-diffusion-v1\model.ckpt` (path from application's folder).
