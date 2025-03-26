using PlayFab.EventsModels;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string musicVolumeParam = "Music Volume";
    [SerializeField] string sfxVolumeParam = "SFX Volume";
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    void Start()
    {
        LoadMusicVolume(); 
        LoadSFXVolume();
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat(musicVolumeParam, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(musicVolumeParam, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat(sfxVolumeParam, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(sfxVolumeParam, volume);
        PlayerPrefs.Save();

    }

    public void LoadMusicVolume()
    {
        if(PlayerPrefs.HasKey(musicVolumeParam))
        {
            float volume = PlayerPrefs.GetFloat(musicVolumeParam);
            musicSlider.value = volume;
            SetMusicVolume();
        }
    }

    public void LoadSFXVolume()
    {
        if(PlayerPrefs.HasKey(sfxVolumeParam))
        {
            float volume = PlayerPrefs.GetFloat(sfxVolumeParam);
            sfxSlider.value = volume;
            SetSFXVolume();
        }
    }
}
