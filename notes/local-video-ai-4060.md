# Local image-to-video on the user's laptop (RTX 4060 Laptop 8 GB, 16 GB RAM, Windows 11)

<div dir="rtl">

## בקצרה (בעברית)
- **אפשר לנסות, אבל זה על הקצה.** מה שמגביל זה 16GB הזיכרון הרגיל, לא הכרטיס הגרפי.
- **מודל ראשי:** Wan 2.2 14B (גרסה מכווצת, GGUF Q4) עם האצה ל-4 צעדים, בתוך ComfyUI.
- **זמן משוער:** 8–15 דקות לקליפ של 5 שניות ברזולוציה 480×832. אחר כך מגדילים את התמונה ומחליקים את התנועה.
- **איכות:** מתחת ל-Kling. רכה יותר, ולפעמים התמונה מתעוותת. צריך 2–4 ניסיונות לכל חדר.
- **חייבים קובץ החלפה (pagefile) גדול:** 48–64GB. ההורדות לוקחות בערך 30GB.
- **קודם בדיקה קצרה של כ-5 דקות.** אם היא קורסת או שצעד אחד לוקח יותר מ-3 דקות, זה לא מעשי על המחשב הזה.
- **הפתרון האמיתי:** להגדיל ל-32GB זיכרון. אחרת, להמשיך עם Kling או Dreamina.

</div>

## Bottom line
Nothing on the list has been shown to work on exactly this PC: an RTX 4060 Laptop with 16 GB RAM. 16 GB of system RAM, not the 8 GB of VRAM, is what decides whether a run works. The whole FramePack family is not practical here. Users with this exact laptop setup measured roughly 26 minutes per second of video, so a 5 s clip takes hours, and the camera barely moves. The one setup worth trying first is Wan 2.2 14B as GGUF Q4 with 4-step Lightning LoRAs in ComfyUI portable, with a 48-64 GB pagefile. I estimate 8-15 minutes per 5 s clip at 480x832 and 16 fps, which still needs upscaling and frame interpolation. Even then, quality is clearly below Kling: softer, sometimes static, sometimes warped, and you should plan on 2-4 takes per room. Have Claude Code run a 33-frame smoke test first, about 5 minutes, to measure real speed before downloading anything else. If it crashes at load even with a 64 GB pagefile and the --disable-pinned-memory / --disable-dynamic-vram flags, or a single step takes more than about 3 minutes, local video on this laptop is not practical. The real fix is a 32 GB RAM upgrade, and the honest alternative is to keep using Kling or another cloud service for finished listing videos. If it does work, freeze that ComfyUI version, because current releases have open memory-streaming bugs.

## Primary: Wan 2.2 I2V-A14B (two experts, high-noise + low-noise) as GGUF Q4_K_M + lightx2v 4-step LoRAs (the 1022 release), in ComfyUI Windows portable with city96 ComfyUI-GGUF. Claude Code drives it headless through ComfyUI's HTTP API.

**Install (Claude Code on the PC runs this in PowerShell):**

Claude Code runs all of this in PowerShell; the user only clicks UAC prompts.
0) Make sure the NVIDIA driver is current, R580 or newer (check with nvidia-smi; update through the NVIDIA App if needed). The current portable build ships Python 3.13 + PyTorch CUDA 13.0, and the ComfyUI README says 'Update your Nvidia drivers if it doesn't start.'
1) winget install -e --id 7zip.7zip ; winget install -e --id Git.Git
2) mkdir C:\AI ; curl.exe -L -o C:\AI\cui.7z https://github.com/Comfy-Org/ComfyUI/releases/latest/download/ComfyUI_windows_portable_nvidia.7z ; & 'C:\Program Files\7-Zip\7z.exe' x C:\AI\cui.7z -oC:\AI  (latest is v0.39.0, 5 Oct 2026; use a short path on the internal NVMe)
3) cd C:\AI\ComfyUI_windows_portable ; git clone https://github.com/city96/ComfyUI-GGUF ComfyUI/custom_nodes/ComfyUI-GGUF ; .\python_embeded\python.exe -s -m pip install -r .\ComfyUI\custom_nodes\ComfyUI-GGUF\requirements.txt  (these are the exact commands from the city96 README). Optional: git clone Fannovel16/ComfyUI-Frame-Interpolation for RIFE.
4) .\python_embeded\python.exe -m pip install -U huggingface_hub ; then use 'hf download' to put these files in place:
   models\unet: QuantStack/Wan2.2-I2V-A14B-GGUF HighNoise/Wan2.2-I2V-A14B-HighNoise-Q4_K_M.gguf (9.65 GB) and LowNoise/...-LowNoise-Q4_K_M.gguf (about 9.65 GB)
   models\text_encoders: Comfy-Org/Wan_2.1_ComfyUI_repackaged split_files/text_encoders/umt5_xxl_fp8_e4m3fn_scaled.safetensors (about 6.7 GB)
   models\vae: Comfy-Org/Wan_2.2_ComfyUI_Repackaged split_files/vae/wan_2.1_vae.safetensors (about 0.25 GB)
   models\loras: lightx2v/Wan2.2-Distill-Loras wan2.2_i2v_A14b_high_noise_lora_rank64_lightx2v_4step_1022.safetensors and the matching low_noise file (roughly 0.6 GB each)
5) Start with .\run_nvidia_gpu.bat. Claude Code opens the built-in 'Wan2.2 14B I2V' template, swaps both 'Load Diffusion Model' nodes for 'Unet Loader (GGUF)', saves it as API-format JSON, then POSTs jobs to http://127.0.0.1:8188/prompt. Output lands in ComfyUI\output. Keep the browser tab closed during runs to save RAM.
6) Windows tweaks: under Settings > Display > Graphics, set python.exe to High performance (the RTX 4060, not the Intel UHD). In NVIDIA Control Panel, set CUDA Sysmem Fallback Policy to Prefer No Sysmem Fallback. Keep the laptop plugged in with the vendor Performance/Turbo mode on.
7) Once a run works, pin that ComfyUI version (don't run update) because recent releases have open memory-streaming bugs.

**Download / disk:** About 30 GB downloaded: about 2 GB portable 7z + about 27.5 GB of models. About 35-38 GB on disk after extraction, plus a 48-64 GB pagefile, so roughly 100 GB of the about 160 GB free. Optional fallback 5B files add about 7 GB.

**Time per clip:** Estimated about 8-15 min per 5 s clip once warmed up; the first run is about 15-25 min while it reads about 27 GB from the SSD. Nobody has measured this on 16 GB RAM, so these are estimates. Basis: a measured 40.1 s/it for Wan 2.2 I2V 640x480x81 with the 4-step LoRA on an RTX 3070 Ti 8 GB laptop (ComfyUI #11081, but with 64 GB RAM). Scaled to 480x832 plus GGUF dequant cost, that gives about 65-80 s/it, so 4 steps is about 4.5-5.5 min. Add the expert swap and SSD re-reads (16 GB can't cache both 9.65 GB experts), umt5 encode, and about 1-2 min of VAE decode. If a single step takes more than about 3 min, Windows is thrashing the pagefile; go to the Q3_K_M or fallback step instead of waiting.

**Resolution / finishing:** 480x832 (WxH, about 9:16; multiples of 16), 81 frames at 16 fps = 5.06 s. Finishing steps: RIFE x2 to 32 fps, or ffmpeg to 30 fps. Then upscale with ffmpeg lanczos to 1108x1920 and center-crop to 1080x1920 (an ESRGAN pass is sharper but costs many more minutes). For 10 s, don't generate 161 frames: either RIFE x4 the 81 frames and play at 32 fps, which makes a slower 10 s glide and interpolates cleanly on slow camera moves, or chain a second clip from the last frame, which drifts more.

**Settings:**

First run a smoke test at 480x832 with 33 frames and 4 steps to measure s/it before any real clip. Pre-crop the photo to 9:16 (ffmpeg/PIL) so WanImageToVideo doesn't stretch it.
WanImageToVideo: 480x832, length 81, batch 1.
Two KSamplerAdvanced nodes: euler/simple, CFG 1.0, 4 total steps. High-noise expert runs steps 0-2 with add_noise on and return leftover noise on; low-noise expert runs 2-4 with add_noise off. Keep the template's ModelSamplingSD3 shift. Both LoRAs at strength 1.0. Use a fixed seed and make 2-4 takes per photo with different seeds.
Prompt: 'Slow, smooth dolly-in: the camera glides steadily forward into the room at walking pace. Static empty interior, nothing moves except the camera, straight walls and furniture stay rigid, natural daylight, realistic.' At CFG 1 the negative prompt is ignored, so don't rely on it.
If clips come out near-static (a known Lightning side effect): set the high-noise LoRA to 0.7-0.8, CFG 2-3 on the high-noise pass, 6-8 total steps (about 1.5-2x the time), or use the 3-sampler split (first high steps without the LoRA at CFG 3.5).
Launch flags: start with defaults. If you see 'HostBuffer.read_file_slice failed' or OOM while loading weights (ComfyUI #14250, #15255, still open), add --disable-pinned-memory; if it persists, add --disable-dynamic-vram.
If RAM crashes persist: switch to the Q3_K GGUF experts and the city96 umt5-xxl GGUF Q5_K_M text encoder (via the CLIPLoader (GGUF) node, type wan).
Don't try 720x1280 on this laptop; VAE decode is likely to OOM and each step would take about 3x longer.

**Pagefile:** Required. Set a custom pagefile on C: (the internal NVMe), initial 49152 MB and maximum 65536 MB, with 'Automatically manage' unchecked, then reboot. Use System Properties > Advanced > Performance > Virtual memory, or an admin PowerShell setting Win32_ComputerSystem.AutomaticManagedPagefile=$false and Win32_PageFileSetting InitialSize/MaximumSize; the user must approve UAC. Evidence: ComfyUI #10140, an 8 GB GPU with 16 GB RAM running the smaller Wan 2.2 5B, crashed silently at a 30 GB pagefile and worked at 50 GB. Expect tens of GB of SSD writes per session and a near-unusable PC during runs. Upgrading to 32 GB RAM (most i7-13620H gaming laptops have two DDR5 SO-DIMM slots; check the model) is the single most effective fix.

**Quality vs Kling:** Clearly below Kling. Native output is 480p and 16 fps, so after upscaling and interpolation it looks acceptable on a phone in Reels/TikTok but soft next to Kling's 1080p. Faithfulness to the real photo is good at the start of the clip. A forward move forces the model to invent unseen parts of the room, so expect bent door frames, warping furniture edges and smeared textures in some takes, worse the farther the camera travels. Keep the move slow and short. The 4-step Lightning LoRAs damp motion: some takes will barely move and some will look slow-motion, which is partly fine for a glide. Expect roughly 1 usable clip per 2-4 generations. Q4 quantization softens fine detail a little. Kling gives more reliable camera paths and cleaner geometry on the first try.

## Fallback: Wan 2.2 TI2V-5B as GGUF Q8_0, in the same ComfyUI install (official 'Wan2.2 5B' template, Unet Loader (GGUF), VAE Decode (Tiled)); 704x1280 native 9:16, 121 frames at 24 fps (5 s)

Use it if the 14B route crashes at load even with the 64 GB pagefile, or a step takes more than about 3 min because of RAM thrashing. It is one model with no expert swap and about half the RAM footprint: a 5.4 GB GGUF plus a 1.4 GB VAE, reusing the umt5 encoder. It is also the only model with a public exact-match report (8 GB GPU + 16 GB RAM, ComfyUI #10140) that ran once the pagefile was 50 GB. It outputs native vertical 704x1280 at 24 fps, so no interpolation is needed. Costs: noticeably softer detail and more warping than 14B, and dropping below 720p hurts it a lot. It is also not fast: with template default steps (about 20, CFG 5) I extrapolate about 15-30 min per 5 s clip including tiled VAE decode, with no measured 4060 Laptop figure. Kijai's FastWan 5B LoRA or the hum-ma Turbo GGUF at 4-6 steps and CFG 1 could cut that to about 8-12 min, but that is untested for image-to-video.

**Install:** In C:\AI\ComfyUI_windows_portable: hf download QuantStack/Wan2.2-TI2V-5B-GGUF Wan2.2-TI2V-5B-Q8_0.gguf into ComfyUI\models\unet; hf download Comfy-Org/Wan_2.2_ComfyUI_Repackaged split_files/vae/wan2.2_vae.safetensors into ComfyUI\models\vae (the 5B needs this VAE, not wan_2.1_vae). Reuse umt5_xxl_fp8_e4m3fn_scaled. Open the 'Wan2.2 5B' video template, swap the loader for 'Unet Loader (GGUF)' and the decoder for 'VAE Decode (Tiled)', and set Wan22ImageToVideoLatent to 704x1280, length 121. Same pagefile and the same --disable-pinned-memory / --disable-dynamic-vram escape hatches. Extra download about 7 GB.

## Rejected (and why)
- FramePack original / F1 official one-click package: rejected. Verified in issue #62: an RTX 4060 laptop 8 GB with 16 GB RAM needed a 32 GB+ pagefile just to load and then ran at about 100 s/it. A second user with 'exactly same setup' reported 40.67 s/it and '26 min' per second of video, so a 5 s clip takes hours. Camera motion is also 'very subtle', which is the wrong fit for a forward glide.
- FramePack Studio / FramePack-eichi: same 13B HunyuanVideo engine and the same RAM wall. eichi's own README estimates 36-45 GB RAM (70-90 GB with LoRAs), and eichi's provenance is now uncertain. Rejected.
- FramePack via kijai ComfyUI-FramePackWrapper (fp8): lowest-RAM FramePack route on paper, but the wrapper is marked WIP, there is no 16 GB success report, a 4060 user got black output, and it still has FramePack's weak camera motion. Rejected.
- Wan 2.2 14B 'Rapid AllInOne' (Phr00t / befox GGUF): its only 4060 success (111 s for a ~2 s 480x480 clip) was a desktop 4060 with 32 GB RAM. The merge is deprecated and its license is unverified. The Cordux low-VRAM workflow's GGUF files are the 't2v ... nsfw' v10 build, a poor fit for a business, and its '~2.5 min on 4060' is an unmeasured estimate for a 1 s clip. Keep only as a third try.
- Wan 2.1 I2V-14B-480P GGUF + lovis93 'Push-in camera' LoRA: the LoRA is a drone-style push-in for the 720p Wan 2.1 model (its example is the camera rushing into a lavender field), probably too fast for a slow interior glide. Wan 2.1 is also behind 2.2 in motion quality. Worth trying only if prompt-driven glides on 2.2 won't move.
- WanGP / Wan2GP: a credible alternative UI with low-RAM profile 5, --perc-reserved-mem-max 0.2-0.3 and an MCP/headless mode. But there is no 16 GB RAM report, the docs only say 'Enable swap file', and it changes almost daily (v18.00 on 11 Oct 2026), which makes repeatable Claude Code automation riskier than a pinned ComfyUI version with a fixed API workflow. Second-line option, not first.
- LightX2V framework: the only project that explicitly claims '8GB VRAM + 16GB RAM', but no independent confirmation. The Windows bundle is on Quark Cloud and is 'not a verified deployment', and the fast kernels are fiddly on Windows. Rejected for a non-technical user.
- Wan 2.2 Fun A14B Control-Camera: deterministic dolly/zoom, but it is the same two 14B experts plus control overhead. Its official example used about 84% of a 24 GB card, and there is no 8 GB report. Too heavy as a first try.
- Wan 2.1 Fun Control-Camera 1.3B: gives an explicit 'Zoom In' forward-translation path and fits easily, but 1.3B detail is too low for showing a rental listing. Last resort only if no 14B/5B take produces any forward motion.
- LTX-2.3 / LTX-2.5 22B: best speed/quality ratio when you have 32 GB+ RAM, but the Gemma-3-12B encoder alone is 9.5-13 GB even quantized, and Kijai says it needs 'at least 32GB' RAM. No 16 GB success found. Rejected.
- LTX-Video 0.9.8 2B distilled: fits easily and is fast, but drifts and morphs on real photos, and there is a report of a visible watermark on the fp8 build. Drafts only.
- LTX-Video 0.9.8 13B: superseded by LTX-2 per its own README, fp8 checkpoint + T5 is about 20.6 GB (more than RAM), and there is no 8 GB report. Rejected.
- HunyuanVideo 1.5 8.3B: the official minimum is 14 GB VRAM, DiT + Qwen2.5-VL-7B encoder is about 16-18 GB at fp8 (more than RAM), and there is no 8 GB report. Rejected.
- CogVideoX-5B-I2V / 1.5: dated. The 5B is landscape-only at 720x480 and 8 fps; 1.5 is about 5x heavier. Rejected.
- SVD-XT 1.1: no text prompt, so you can't ask for a forward glide; 25 frames, landscape-trained. Rejected.
- LTX-VideoQ8: the only exact-GPU benchmark, but it needs a q8_kernels build from source with MSVC + CUDA toolkit on Windows, and the model is from late 2024. Rejected.
- FastH3 Trim / Kandinsky 6 Lite: days old. FastH3 was tested on a 4090 capped to 8 GB, not a real 8 GB card, and its 18.9 GB int8 file exceeds RAM. Kandinsky 6 Lite has no memory data. Rejected for now.

## Fact-check notes
- lilting.ch's Wan 2.2 '111 seconds' result was a desktop RTX 4060 8 GB with 32 GB RAM, Windows 11, portable ComfyUI with --lowvram, making about a 2 s clip with the 14B Rapid distilled model. It was not a laptop and not 16 GB, and the author credits the 32 GB for the offloading. I could not open lilting.ch from this environment (blocked), so the '4060 Laptop' label on its FramePack-F1 56-min test comes only from search snippets and is unverified.
- ComfyUI #11081 is verified, with precise numbers: an RTX 3070 Ti 8 GB laptop with 64 GB RAM ran Wan 2.2 I2V 640x480x81 at 40.13 s/it (fp16 + LoRA) and 41.99 s/it (fp8 + LoRA), using 4.2-5.1 GB VRAM. Those speeds depend on 64 GB RAM caching the weights; they are not a 16 GB figure.
- ComfyUI #10140 is verified: an RTX 2060 SUPER 8 GB with 16 GB RAM (log: 16197 MB), portable v0.3.61, running Wan 2.2 TI2V-5B in fp16 (not GGUF), crashed silently 2-5 s into sampling even with --lowvram. Quote: '16GB of RAM and 30GB of page file isn't enough, 50GB did the trick.'
- ComfyUI PR #13802 (new multi-threaded load-from-disk, merged 21 May 2026) was tested on a Windows RTX 5060 with 16 GB RAM, including Wan 2.2, so 16 GB is a developer-tested configuration. The Wan timings are only in a chart, though, and post-merge comments report both speedups and regressions (models reloading from disk).
- ComfyUI #15255 ('HostBuffer.read_file_slice failed' then CUDA OOM, Dynamic VRAM) is still open as of early October 2026 across versions 0.30-0.37, and includes Wan 2.2 fp8 users. Workarounds are --disable-dynamic-vram or --disable-pinned-memory, and some users downgraded to 0.36. #14250 is also still open.
- The current ComfyUI Windows portable ships Python 3.13 + PyTorch CUDA 13.0, so an older laptop NVIDIA driver will stop it from starting. Updating the driver is a real install step that the candidate lists skipped.
- The lightx2v 4-step I2V LoRA filenames are wan2.2_i2v_A14b_high_noise_lora_rank64_lightx2v_4step_1022.safetensors and the matching low_noise file (confirmed via comfy.org model pages). The older 'wan2.2_i2v_lightx2v_4steps_lora_v1_*' names may not match current templates.
- With the 4-step Lightning setup at CFG 1, negative prompts have no effect, so negative-prompt advice like 'warping, distorted furniture' does nothing in that configuration.
- QuantStack HighNoise Q4_K_M is confirmed at 9.65 GB; the LowNoise Q4_K_M matches at about 9.65 GB (via a mirror). The Q3_K sizes of '7-8 GB each' are unverified.
- Wan2GP: the README gives no RAM minimum and calls Profile 5 'Good old failsafe'. The troubleshooting doc's low-RAM advice is '--profile 5', '--perc-reserved-mem-max 0.3' and 'Enable swap file (OS level)', with no pagefile size. Latest is v18.00, 11 Oct 2026.
- FramePack issue #62 is confirmed: an RTX 4060 laptop 8 GB with 16 GB RAM ran at about 100 s/it with a 32 GB pagefile. A same-setup commenter reported 40.67 s/it and 'for each second it is taking 26 min' (the 80 GB pagefile was a suggestion in that thread).
- The lovis93 'Push-in camera' LoRA is trained on 100 clips for Wan 2.1 I2V 720p and is described as drone-style push-in motion, not a slow interior dolly.
- The Cordux 'Wan2.2 low-VRAM' workflow actually uses befox 'wan2.2-t2v-rapid-aio-v10-nsfw' GGUF files. The candidate's advice to 'use the SFW variant' isn't available in that GGUF set, and its 4060 time is an unmeasured estimate for a 1 s 512x512 clip.
- 480x832 is not exactly 9:16 (0.577 vs 0.5625). Crop about 2.5% of the width when finishing to 1080x1920, or generate at 480x848.
