using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public AudioPeer audioPeer;
    public bool useBuffer;
    public float startScale;
    public float maxScale;

	void Update ()
    {
        if (!useBuffer)
            transform.localScale = new Vector3(audioPeer.amplitude * maxScale + startScale, audioPeer.amplitude * maxScale + startScale, audioPeer.amplitude * maxScale + startScale);
        else
            transform.localScale = new Vector3(audioPeer.amplitudeBuffer * maxScale + startScale, audioPeer.amplitudeBuffer * maxScale + startScale, audioPeer.amplitudeBuffer * maxScale + startScale);
    }
}
