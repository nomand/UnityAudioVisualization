using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource audioSource;
    public static float[] samples = new float[512];
    public static float[] frequencyBand = new float[8];
    public static float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];

    public int frequencyBands = 8;

	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        frequencyBand = new float[frequencyBands];
	}
	
	void Update ()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
	}

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        //0 - 20 - 60 - 250 - 500 - 2000 - 4000 - 6000 - 20000

        int count = 0;
        float average = 0;

        for(int i = 0; i < frequencyBands; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7) { sampleCount += 2; }

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count+1);
                count++;
            }

            average /= count;
            frequencyBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        for(int i = 0; i < frequencyBands; i++)
        {
            if(frequencyBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = frequencyBand[i];
                bufferDecrease[i] = 0.005f;
            }
            if(frequencyBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }
}