using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliders : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start() {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    public void SetMusicVolume(float volume) {
        SetVolume("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume) {
        SetVolume("SFXVolume", volume);
    }

    private void SetVolume(string name, float volume) {
        float db = Mathf.Log(Mathf.Max(volume, 0.001f)) * 20;
        mixer.SetFloat(name, db);
        PlayerPrefs.SetFloat(name, volume);
    }
}
