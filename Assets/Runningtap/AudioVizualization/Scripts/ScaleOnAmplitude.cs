using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public Runningtap.AnalyzeAudio audioData;
    public bool useBuffer;
    public float startScale;
    public float maxScale;

	void Update ()
    {
        var ampScale = audioData.Amplitude * maxScale + startScale;
        var ampBufferScale = audioData.AmplitudeBuffer * maxScale + startScale;

        if (!useBuffer)
            transform.localScale = new Vector3(1, 1, 1) * ampScale;
        else
            transform.localScale = new Vector3(1, 1, 1) * ampBufferScale;
    }
}