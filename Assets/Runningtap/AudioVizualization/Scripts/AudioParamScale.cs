using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioParamScale : MonoBehaviour
{
    public Runningtap.AnalyzeAudio audioData;
    public int band = 0;
    public float sensitivity = 1;
    public bool useBuffer = true;

    public enum ScaleDriver
    {
        Amplitude,
        AudioBand
    }

    public ScaleDriver scaleDriver;

    [Tooltip("Initial Scale")]
    public Vector3 startScale = new Vector3(1,1,1);
    [Tooltip("Axis magnitude for driving scale")]
    public Vector3 Axis = new Vector3(1,1,1);

    private float drivenValue;

    private void Start()
    {
        if(transform.localScale != startScale)
        {
            transform.localScale = startScale;
        }

        switch (scaleDriver)
        {
            case ScaleDriver.Amplitude: scaleDriver = ScaleDriver.Amplitude; break;
            case ScaleDriver.AudioBand: scaleDriver = ScaleDriver.AudioBand; break;
        }
    }

    void Update ()
    {
        if(scaleDriver == ScaleDriver.AudioBand)
        {
            drivenValue = useBuffer ? audioData.AudioBandBuffer[band] * sensitivity : audioData.AudioBand[band] * sensitivity;
        }
        else if(scaleDriver == ScaleDriver.Amplitude)
        {
            drivenValue = useBuffer ? audioData.AmplitudeBuffer * sensitivity : audioData.Amplitude * sensitivity;
        }

        transform.localScale = new Vector3(Axis.x * drivenValue, Axis.y * drivenValue, Axis.z * drivenValue) + startScale;
	}
}