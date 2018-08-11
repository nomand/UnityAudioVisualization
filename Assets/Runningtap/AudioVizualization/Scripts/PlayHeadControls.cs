using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayHeadControls : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;

	void Start ()
    {
        slider.minValue = 0;
        slider.maxValue = (audioSource.clip.length * audioSource.clip.frequency * audioSource.clip.channels) - 1;
    }

    void Update()
    {
        slider.value = audioSource.time * audioSource.clip.frequency * audioSource.clip.channels;
    }

    public void Scrub()
    {
        audioSource.time = slider.value / (audioSource.clip.frequency * audioSource.clip.channels);
    }
}