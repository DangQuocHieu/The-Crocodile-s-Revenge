using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
        Observer.AddObserver(GameEvent.OnGameStart, PlayStartSFX);
        Observer.AddObserver(GameEvent.OnGamePaused, PauseSFX);
        Observer.AddObserver(GameEvent.OnGameResume, ResumeSFX);
        Observer.AddObserver(GameEvent.OnPlayerPickUpCoin, PlayCoinSFX);
        Observer.AddObserver(GameEvent.OnPlayerPickUpPowerup, PlayPowerupSFX);
        Observer.AddObserver(GameEvent.OnPowerdown, PlayPowerdownSFX);
        Observer.AddObserver(GameEvent.OnObstacleHitPlayer, PlayHurtSFX);
        Observer.AddObserver(GameEvent.OnPlayerJump, PlayJumpSFX);
        Observer.AddObserver(GameEvent.OnGameOver, PlayGameOverSFX);
        Observer.AddObserver(GameEvent.OnPlayerMultipleJump, PlayMultipleJumpSFX);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, PlayHurtSFX);
        Observer.AddObserver(GameEvent.OnPlayerBeginRevive, PauseMusic);
        Observer.AddObserver(GameEvent.OnPlayerFinishRevive, ContinuePlayMusic);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameStart, PlayStartSFX);
        Observer.RemoveListener(GameEvent.OnGamePaused, PauseSFX);
        Observer.RemoveListener(GameEvent.OnGameResume, ResumeSFX);
        Observer.RemoveListener(GameEvent.OnPlayerPickUpCoin, PlayCoinSFX);
        Observer.RemoveListener(GameEvent.OnPlayerPickUpPowerup, PlayPowerupSFX);
        Observer.RemoveListener(GameEvent.OnPowerdown, PlayPowerdownSFX);
        Observer.RemoveListener(GameEvent.OnObstacleHitPlayer, PlayHurtSFX);
        Observer.RemoveListener(GameEvent.OnPlayerJump, PlayJumpSFX);
        Observer.RemoveListener(GameEvent.OnPlayerMultipleJump, PlayMultipleJumpSFX);
        Observer.RemoveListener(GameEvent.OnGameOver, PlayGameOverSFX);
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, PlayHurtSFX);
        Observer.RemoveListener(GameEvent.OnPlayerBeginRevive, PauseMusic);
        Observer.RemoveListener(GameEvent.OnPlayerFinishRevive, ContinuePlayMusic);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu Scene")
        {
            PlayMusic(AudioName.MenuMusic);
        }
        else if (scene.name == "Game Scene")
        {
            PlayMusic(AudioName.GameMusic);
        }
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
        foreach(var audio in musicAudios)
        {
            AudioSource AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSource.clip = audio.clip;
            AudioSource.loop = true;
            AudioSource.volume = audio.volume;
            AudioSource.pitch = audio.pitch;
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

    void PlayGameOverSFX(object[] datas)
    {
        float duration = (float)datas[0];
        StartCoroutine(PlayGameOverSFXCoroutine(duration));
    }
    
    IEnumerator PlayGameOverSFXCoroutine(float duration)
    {
        musicSource.Stop();
        yield return new WaitForSecondsRealtime(duration);
        AudioListener.pause = false;
        PlaySFX(AudioName.GameOverSFX);
    }
    public void PlayStartSFX(object[] datas)
    {
        musicSource.Stop();
        PlaySFX(AudioName.GameStartSFX);
    }

}
