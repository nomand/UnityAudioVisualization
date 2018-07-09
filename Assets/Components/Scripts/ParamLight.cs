using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Light))]
public class ParamLight : MonoBehaviour {

    public AudioPeer audioPeer;
    Light myLight;
    public float minBrightness = 0;
    public float maxBrightness = 10;
    public int band = 4;

	void Start ()
    {
        myLight = GetComponent<Light>();
	}
	
	void Update ()
    {
        myLight.intensity = (audioPeer.AudioBandBuffer[band] * (maxBrightness - minBrightness)) + minBrightness;
	}
}