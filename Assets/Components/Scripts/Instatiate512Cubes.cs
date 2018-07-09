using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instatiate512Cubes : MonoBehaviour {

    public AudioPeer audioPeer;

    public GameObject sampleCubePrefab;
    public float radius = 100f;
    public float scale = 2f;

    private GameObject[] sampleCube = new GameObject[512];

	void Start ()
    {
		for(int i = 0; i < 512; i++)
        {
            GameObject instance = Instantiate(sampleCubePrefab);
            instance.transform.position = transform.position;
            instance.transform.parent = transform;
            instance.name = "SampleCube" + i;
            transform.eulerAngles = new Vector3(0, -0.703125f * i);
            instance.transform.position = Vector3.forward * radius;
            instance.transform.eulerAngles = new Vector3(90, 0, 0);
            sampleCube[i] = instance;
        }
	}
	
	void Update ()
    {
		for(int i = 0; i < audioPeer.FrequencyBands; i++)
        {
            sampleCube[i].transform.localScale = new Vector3(10, audioPeer.AudioBandBuffer[i] * scale + 10, 10);
        }
    }
}