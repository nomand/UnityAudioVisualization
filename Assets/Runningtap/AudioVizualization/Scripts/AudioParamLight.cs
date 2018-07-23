using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Light))]
public class AudioParamLight : MonoBehaviour
{
    public Runningtap.AnalyzeAudio audioData;
    
    public float minBrightness = 0;
    public float maxBrightness = 10;
    public int band = 4;

    private Light myLight;

    void Start ()
    {
        myLight = GetComponent<Light>();
	}
	
	void Update ()
    {
        myLight.intensity = (audioData.AudioBandBuffer[band] * (maxBrightness - minBrightness)) + minBrightness;
	}
}