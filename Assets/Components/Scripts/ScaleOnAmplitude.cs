using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public bool useBuffer;
    public float startScale;
    public float maxScale;

	void Update ()
    {
        if (!useBuffer)
            transform.localScale = new Vector3(AudioPeer.amplitude * maxScale + startScale, AudioPeer.amplitude * maxScale + startScale, AudioPeer.amplitude * maxScale + startScale);
        else
            transform.localScale = new Vector3(AudioPeer.amplitudeBuffer * maxScale + startScale, AudioPeer.amplitudeBuffer * maxScale + startScale, AudioPeer.amplitudeBuffer * maxScale + startScale);
    }
}
