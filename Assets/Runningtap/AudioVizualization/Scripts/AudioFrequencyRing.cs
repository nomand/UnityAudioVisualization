using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFrequencyRing : MonoBehaviour
{

    public Runningtap.AnalyzeAudio audioData;

    public GameObject sampleCubePrefab;
    public Vector3 StartScale = new Vector3(1,1,1);
    public float Radius = 100f;
    public float Sensitivity = 2f;

    private GameObject[] sampleCube;

	void Start ()
    {
        sampleCube = new GameObject[audioData.FrequencyBands];
        float angle = 360f / audioData.FrequencyBands;

		for(int i = 0; i < audioData.FrequencyBands; i++)
        {
            GameObject instance = Instantiate(sampleCubePrefab);
            instance.transform.position = transform.position;
            instance.transform.parent = transform;
            instance.name = "SampleCube_" + i;
            transform.eulerAngles = new Vector3(0, -angle * i, 0);
            instance.transform.position = Vector3.forward * Radius;
            instance.transform.eulerAngles = new Vector3(90, 0, 0);
            sampleCube[i] = instance;
        }
	}
	
	void Update ()
    {
		for(int i = 0; i < audioData.FrequencyBands; i++)
        {
            sampleCube[i].transform.localScale = new Vector3(StartScale.x, audioData.AudioBandBuffer[i] * Sensitivity + StartScale.y, StartScale.z);
        }
    }
}