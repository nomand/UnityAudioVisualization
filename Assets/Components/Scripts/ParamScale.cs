using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamScale : MonoBehaviour
{
    public AudioPeer audioPeer;
    public int band = 0;
    public float sensitivity = 1;
    public bool useBuffer = true;

    [Tooltip("Initial Scale")]
    public Vector3 startScale = new Vector3(1,1,1);
    [Tooltip("Axis magnitude for driving scale")]
    public Vector3 Axis = new Vector3(1,1,1);

    private void Start()
    {
        if(transform.localScale != startScale)
        {
            transform.localScale = startScale;
        }
    }

    void Update ()
    {
        var value = useBuffer ? audioPeer.AudioBandBuffer[band] * sensitivity: audioPeer.AudioBand[band] * sensitivity;

        transform.localScale = new Vector3(Axis.x * value, Axis.y * value, Axis.z * value) + startScale;
        Color color = new Color(audioPeer.AudioBandBuffer[band], audioPeer.AudioBandBuffer[band], audioPeer.AudioBandBuffer[band]);
	}
}