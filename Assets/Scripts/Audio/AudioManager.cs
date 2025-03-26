using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;

    [Header("SFX Audio")]
    [SerializeField] Audio[] sfxAudios;
    private Dictionary<AudioName, AudioSource> sfxAudioSourcePool;
    [Header("Music Audio")]
    [SerializeField] Audio[] musicAudios;
    private AudioSource musicSource;
    private float previousPlayTime = 0f;
    private float sfxCooldown = 0.08f;
    bool disableSFX;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeAudioSourcePool();
        ApplyAudioMixer();
        Observer.AddObserver(GameEvent.OnLoginSuccessfully, PlayMenuMusic);
        Observer.AddObserver(GameEvent.OnGameStart, PlayGameMusic);
        Observer.AddObserver(GameEvent.OnGamePaused, PauseSFX);
        Observer.AddObserver(GameEvent.OnGameResume, ResumeSFX);
        Observer.AddObserver(GameEvent.OnPlayerPickUpCoin, PlayCoinSFX);
        Observer.AddObserver(GameEvent.OnPlayerPickUpPowerup, PlayPowerupSFX);
        Observer.AddObserver(GameEvent.OnPowerdown, PlayPowerdownSFX);
        Observer.AddObserver(GameEvent.OnObstacleHitPlayer, PlayHurtSFX);
        Observer.AddObserver(GameEvent.OnPlayerJump, PlayJumpSFX);
        Observer.AddObserver(GameEvent.OnPlayerMultipleJump, PlayMultipleJumpSFX);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, PlayHurtSFX);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, PauseMusic);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, ContinuePlayMusic);
        Observer.AddObserver(GameEvent.OnVehicleWarning, PlayWarningSFX);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnLoginSuccessfully, PlayMenuMusic);
        Observer.RemoveListener(GameEvent.OnGameStart, PlayGameMusic);
        Observer.RemoveListener(GameEvent.OnGamePaused, PauseSFX);
        Observer.RemoveListener(GameEvent.OnGameResume, ResumeSFX);
        Observer.RemoveListener(GameEvent.OnPlayerPickUpCoin, PlayCoinSFX);
        Observer.RemoveListener(GameEvent.OnPlayerPickUpPowerup, PlayPowerupSFX);
        Observer.RemoveListener(GameEvent.OnPowerdown, PlayPowerdownSFX);
        Observer.RemoveListener(GameEvent.OnObstacleHitPlayer, PlayHurtSFX);
        Observer.RemoveListener(GameEvent.OnPlayerJump, PlayJumpSFX);
        Observer.RemoveListener(GameEvent.OnPlayerMultipleJump, PlayMultipleJumpSFX);
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, PlayHurtSFX);
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, PauseMusic);
        Observer.RemoveListener(GameEvent.OnPlayerFinishRevive, ContinuePlayMusic);
        Observer.RemoveListener(GameEvent.OnVehicleWarning, PlayWarningSFX);
        
    }

    void ApplyAudioMixer()
    {
        foreach (var source in sfxAudioSourcePool.Values)
        {
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        }
        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0]; 
    }
    void InitializeAudioSourcePool()
    {
        sfxAudioSourcePool = new Dictionary<AudioName, AudioSource>();
        foreach(var audio in sfxAudios)
        {
            AudioSource AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSource.clip = audio.clip;
            AudioSource.loop = false;
            AudioSource.volume = audio.volume;
            AudioSource.pitch = audio.pitch;
            AudioSource.priority = audio.priority;
            sfxAudioSourcePool.Add(audio.audioName, AudioSource);   
        }
        musicSource = gameObject.AddComponent<AudioSource>();
    }
    public void PauseSFX(object[] datas)
    {
        AudioListener.pause = true;
    }

    public void PlayMusic(AudioName name)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        musicSource.UnPause();
        foreach(var audio in musicAudios)
        {
            if(audio.audioName == name)
            {
                musicSource.clip = audio.clip;
                musicSource.volume = audio.volume;
                musicSource.pitch = audio.pitch;
                musicSource.loop = true;
                musicSource.priority = audio.priority;
            }
        }
        AudioListener.pause = false;
        musicSource.Play();
    }

    public void PauseMusic(object[] datas)
    {
        musicSource.Pause();
    }

    public void ContinuePlayMusic(object[] datas)
    {
        musicSource.UnPause();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void ResumeSFX(object[] datas)
    {
        float delayDuration = (float)datas[0];
        StartCoroutine(ResumeSFXCoroutine(delayDuration));
    }

    IEnumerator ResumeSFXCoroutine(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        AudioListener.pause = false;
    }
    public void PlaySFX(AudioName name, bool isCooldown = false)
    {
        AudioSource source = sfxAudioSourcePool[name];
        if (isCooldown)
        {
            float volumeMultiplier = Mathf.Clamp01((Time.time - previousPlayTime) / sfxCooldown);
            source.PlayOneShot(source.clip, source.volume * volumeMultiplier);
            previousPlayTime = Time.time;
        }
        else
        {
            if (source.isPlaying) source.Stop();
            source.PlayOneShot(source.clip);
        }
    }
    public void PlayMenuMusic(object[] datas)
    {
        PlayMusic(AudioName.MenuMusic);
    }
    public void PlayGameMusic(object[] datas)
    {
        PlayMusic(AudioName.GameMusic);
    }
    void PlayCoinSFX(object[] datas)
    {
        PlaySFX(AudioName.CoinCollectedSFX, isCooldown: true);
    }

    void PlayPowerupSFX(object[] datas)
    {
        PlaySFX(AudioName.PowerupSFX);
    }

    void PlayPowerdownSFX(object[] datas)
    {
        PlaySFX(AudioName.PowerdownSFX);
    }

    void PlayHurtSFX(object[] datas)
    {
        PlaySFX(AudioName.HurtSFX);
    }

    void PlayJumpSFX(object[] datas)
    {
        PlaySFX(AudioName.FirstJumpSFX);
    }

    void PlayMultipleJumpSFX(object[] datas)
    {
        PlaySFX(AudioName.MultipleJumpSFX);
    }

    public void PlayGameOverSFX()
    {
        StartCoroutine(PlayGameOverSFXCoroutine(0));
    }
    
    IEnumerator PlayGameOverSFXCoroutine(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        AudioListener.pause = false;
        PlaySFX(AudioName.GameOverSFX);
    }
    public void PlayStartSFX()
    {
        musicSource.Stop();
        PlaySFX(AudioName.GameStartSFX);
    }

    public void PlayWarningSFX(object[] datas)
    {
        PlaySFX(AudioName.WarningSFX);
    }
}
