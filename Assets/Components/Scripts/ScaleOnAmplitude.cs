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
        var ampScale = audioPeer.Amplitude * maxScale + startScale;
        var ampBufferScale = audioPeer.AmplitudeBuffer * maxScale + startScale;

        if (!useBuffer)
            transform.localScale = new Vector3(1, 1, 1) * ampScale;
        else
            transform.localScale = new Vector3(1, 1, 1) * ampBufferScale;
    }
}