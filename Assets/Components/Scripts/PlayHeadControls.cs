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
        slider.maxValue = audioSource.timeSamples * (1 / audioSource.clip.frequency);
    }

    void Update()
    {
        slider.value = audioSource.timeSamples / audioSource.clip.frequency;
        Debug.Log("what?");
    }

    public void Scrub()
    {
        audioSource.time = (int)slider.value;
    }
}