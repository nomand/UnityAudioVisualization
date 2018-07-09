using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public AudioPeer audioPeer;
    public int band;
    public float startScale;
    public float scale;
    public bool useBuffer;
    Material material;

	void Start ()
    {
        material = GetComponent<MeshRenderer>().materials[0];
	}
	
	void Update ()
    {
        transform.localScale = new Vector3(transform.localScale.x, useBuffer ? audioPeer.AudioBandBuffer[band] * scale + startScale : audioPeer.AudioBand[band] * scale + startScale, transform.localScale.z);
        Color color = new Color(audioPeer.AudioBandBuffer[band], audioPeer.AudioBandBuffer[band], audioPeer.AudioBandBuffer[band]);
        material.SetColor("_EmissionColor", color);
	}
}