using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] TMP_Text volText, startVolText;
    [SerializeField] Slider volSlider, startVolSlider;
    private float currVol = 50f;

    private void Start()
    {
        SetVolume(currVol);
    }

    public void SetVolume(float vol)
    {
        if (vol < 1) vol = 0.01f;
        currVol = vol;
        if (volSlider != null) volSlider.value = vol;
        if (startVolSlider != null) startVolSlider.value = vol;
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(vol/100) * 20f);
        if (volText != null) volText.text = System.Math.Round(vol, 1).ToString();
        if (startVolText != null) startVolText.text = System.Math.Round(vol, 1).ToString();
    }
}
