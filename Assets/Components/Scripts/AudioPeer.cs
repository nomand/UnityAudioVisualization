using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource audioSource;

    [HideInInspector]
    public float[] audioBand, audioBandBuffer;
    [HideInInspector]
    public float amplitude, amplitudeBuffer;
    
    private float[] samplesLeft = new float[512];
    private float[] samplesRight = new float[512];

    private float[] frequencyBand;
    private float[] bandBuffer;
    private float[] bufferDecrease;
    private float[] freqBandHighest;

    private float amplitudeHighest;

    public int frequencyBands = 8;
    public float audioProfile;

    public enum Channel
    {
        Stereo,
        Left,
        Right
    }

    public Channel channel = new Channel();

	void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        frequencyBand = new float[frequencyBands];
        bandBuffer = new float[frequencyBands];
        bufferDecrease = new float[frequencyBands];
        freqBandHighest = new float[frequencyBands];

        audioBand = new float[frequencyBands];
        audioBandBuffer = new float[frequencyBands];

        AudioProfile(audioProfile);
    }
	
	void Update ()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        // 20 - 60 - 250 - 500 - 2000 - 4000 - 6000 - 20000

        int count = 0;
        float average = 0;
        //int sampleCount = Mathf.RoundToInt(512 / frequencyBands);

        for(int i = 0; i < frequencyBands; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7) { sampleCount += 2; }

            for (int j = 0; j < sampleCount; j++)
            {
                if (channel == Channel.Stereo)
                {
                    average += (samplesLeft[count] + samplesRight[count]) * (count + 1);
                }
                else if (channel == Channel.Left)
                {
                    average += samplesLeft[count] * (count + 1);
                }
                else
                {
                    average += samplesRight[count] * (count + 1);
                }

                count++;
            }

            //for (int j = 0; j < sampleCount; j++)
            //{
            //    average += samplesLeft[count] + samplesRight[count] * (count + 1);
            //    count++;
            //}

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

    void CreateAudioBands()
    {
        for (int i = 0; i < frequencyBands; i++)
        {
            if (frequencyBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = frequencyBand[i];
            }

            audioBand[i] = frequencyBand[i] / freqBandHighest[i];
            audioBandBuffer[i] = bandBuffer[i] / freqBandHighest[i];
        }
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for(int i = 0; i < frequencyBands; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }
        if(currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }
        amplitude = currentAmplitude / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }

    void AudioProfile(float audioProfile)
    {
        for(int i = 0; i < frequencyBands; i++)
        {
            freqBandHighest[i] = 0;
        }
    }
}