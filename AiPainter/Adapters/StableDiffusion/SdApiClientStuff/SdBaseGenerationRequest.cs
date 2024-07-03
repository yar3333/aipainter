namespace AiPainter.Adapters.StableDiffusion.SdApiClientStuff;

[Serializable]
class SdBaseGenerationRequest
{
    public string prompt { get; set; } = "";
    public string negative_prompt { get; set; } = "";

    public int n_iter { get; set; } = 1;
    public int steps { get; set; } = 50;

    public int width { get; set; } = 512;
    public int height { get; set; } = 512;

    /*
    ('Euler a', 'sample_euler_ancestral', ['k_euler_a'], {}),
    ('Euler', 'sample_euler', ['k_euler'], {}),
    ('LMS', 'sample_lms', ['k_lms'], {}),
    ('Heun', 'sample_heun', ['k_heun'], {}),
    ('DPM2', 'sample_dpm_2', ['k_dpm_2'], {}),
    ('DPM2 a', 'sample_dpm_2_ancestral', ['k_dpm_2_a'], {}),
    ('DPM fast', 'sample_dpm_fast', ['k_dpm_fast'], {}),
    ('DPM adaptive', 'sample_dpm_adaptive', ['k_dpm_ad'], {}),
    ('LMS Karras', 'sample_lms', ['k_lms_ka'], {'scheduler': 'karras'}),
    ('DPM2 Karras', 'sample_dpm_2', ['k_dpm_2_ka'], {'scheduler': 'karras'}),
    ('DPM2 a Karras', 'sample_dpm_2_ancestral', ['k_dpm_2_a_ka'], {'scheduler': 'karras'}),
     */
    public string sampler_index { get; set; } = "Euler";

    public decimal cfg_scale { get; set; } = new(7.5);

    public decimal denoising_strength { get; set; } = 0.75m;

    public long seed { get; set; } = -1;
    public long subseed { get; set; } = -1;
    public decimal subseed_strength { get; set; } = 0;
    public int seed_resize_from_h { get; set; } = -1;
    public int seed_resize_from_w { get; set; } = -1;

    public string[] styles { get; set; } = { };

    public int batch_size { get; set; } = 1;

    public bool tiling { get; set; } = false;
    public bool restore_faces { get; set; } = false;

    // sampler parameters
    public decimal eta { get; set; } = 0; // 0..1
    public decimal s_churn { get; set; } = 0; // 0 or 1 are best
    public decimal s_noise { get; set; } = 1; // best is 1
    public decimal s_tmax { get; set; } = 0; // not much effect to generation
    public decimal s_tmin { get; set; } = 0; // not much effect to generation

    /// <summary>
    /// sd_model_checkpoint: "path to checkpoint"
    /// CLIP_stop_at_last_layers: 2
    /// </summary>
    public SdSettings? override_settings { get; set; } = null;
    public bool? override_settings_restore_afterwards { get; set; } = null;
}