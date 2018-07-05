using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
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
        transform.localScale = new Vector3(transform.localScale.x, useBuffer ? AudioPeer.bandBuffer[band] * scale : AudioPeer.frequencyBand[band] * scale + startScale, transform.localScale.z);
        Color color = new Color(AudioPeer.audioBandBuffer[band], AudioPeer.audioBandBuffer[band], AudioPeer.audioBandBuffer[band]);
        material.SetColor("_EmissionColor", color);
	}
}