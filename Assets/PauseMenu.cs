using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private Transform settingsSliders;
    private AudioManager audioManager;
    private Slider sfxSlider;
    private Slider soundSlider; 

    void Awake()
    {
        settingsSliders = transform.Find("SettingsMenu");
        audioManager = FindObjectOfType<AudioManager>();
        sfxSlider = settingsSliders.Find("SliderSFX").GetComponent<Slider>();
        soundSlider = settingsSliders.Find("SliderMusic").GetComponent<Slider>();
    }

    void Start()
    {
        sfxSlider.value = audioManager.sfxMultiplier;
        soundSlider.value = audioManager.musicMultiplier;
    }

    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");

    public void SettingsMenu() => settingsSliders.gameObject.SetActive(true);
    public void CloseSettings() => settingsSliders.gameObject.SetActive(false);

    public void SetSound(float value) => audioManager.SetSound(value);
    public void SetSfx(float value) => audioManager.SetSfx(value);

    public void Quit() => Application.Quit();
}
