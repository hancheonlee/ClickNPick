using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    public GameObject optionsUI;
    public GameObject settingsButton;
    public GameObject crossUI;
    public GameObject crossUI2;

    public void Start()
    {

        // Load saved values
        LoadSettings();

        // Add listeners to handle slider value changes
        _musicSlider.onValueChanged.AddListener(delegate { OnMusicSliderValueChanged(); });
        _sfxSlider.onValueChanged.AddListener(delegate { OnSFXSliderValueChanged(); });

        AudioManager.Instance.UnmuteMusic();
        crossUI.SetActive(false);
        AudioManager.Instance.UnmuteSFX();
        crossUI2.SetActive(false);
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }

    public void MuteMusic()
    {
        if (AudioManager.Instance.musicSource.mute)
        {
            AudioManager.Instance.UnmuteMusic();
            crossUI.SetActive(false);
        }
        else
        {
            AudioManager.Instance.MuteMusic();
            crossUI.SetActive(true);
        }

    }
    public void MuteSFX()
    {
        if (AudioManager.Instance.sfxSource.mute)
        {
            AudioManager.Instance.UnmuteSFX();
            crossUI2.SetActive(false);
        }
        else
        {
            AudioManager.Instance.MuteSFX();
            crossUI2.SetActive(true);
        }

    }
    public void Open()
    {
        optionsUI.SetActive(true);
        settingsButton.SetActive(false);
        AudioManager.Instance.PlaySFX("Pause");

        Time.timeScale = 0f;

        CameraSystem.free = false;
    }

    public void Resume()
    {
        optionsUI.SetActive(false);
        settingsButton.SetActive(true);
        SaveSettings();
        AudioManager.Instance.PlaySFX("Back");

        Time.timeScale = 1f;

        CameraSystem.free = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("prototype_01");
    }

    public void MainMenu()
    {
        
        SceneManager.LoadScene("Level 0");
    }
    

    private void OnMusicSliderValueChanged()
    {
        MusicVolume();
        PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
        PlayerPrefs.Save();
    }

    private void OnSFXSliderValueChanged()
    {
        SFXVolume();
        PlayerPrefs.SetFloat("SFXVolume", _sfxSlider.value);
        PlayerPrefs.Save();
    }


    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", _sfxSlider.value);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
    }
}
