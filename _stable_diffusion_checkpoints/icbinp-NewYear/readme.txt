Updated - New Year: An updated "full" version of the model, which is just SECO with a lora I made from a curated dataset trained at 1024x1024. I have chucked up some comparisons so you can see what it looks like against other realism models and against older ICBINP versions. I reckon it gives better skin, and isn't as dark/desaturated as the previous couple of versions, while still playing nicely with loras

Recommended settings: DPM++ 3M SDE Karras or DPM++ 2M Karras, 20-30 steps, 2.5-5 CFG (or use Dynamic Thresholding), happiest at 640x960 with a hires fix on top, but if you are happy to hunt through seeds to avoid head duplicates, 768x1152 works quite well also.


Following on from Gorilla With A Brick, I've merged in 10 more photorealistic models at various weights, and some more noise offset to create something that when prompted for photorealism will make you go "I Can't Believe It's Not Photography". It will happily create CGI characters and awesome landscapes as well.

As always, pruned to fp16, the VAE is baked in (SD-v2 840000 - ignore the VAE requirement above as the system is borked!), and the man in the suit can be used as the calibration prompt on any UI!