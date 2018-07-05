using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource audioSource;
    public static float[] samplesLeft = new float[512];
    public static float[] samplesRight = new float[512];
    public static float[] frequencyBand = new float[8];
    public static float[] bandBuffer = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    public static float amplitude;
    public static float amplitudeBuffer;

    public int frequencyBands = 8;
    public float audioProfile;

    float[] bufferDecrease = new float[8];
    float[] freqBandHighest = new float[8];
    float amplitudeHighest;

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

    void CreateAudioBands()
    {
        for(int i = 0; i < frequencyBands; i++)
        {
            if(frequencyBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = frequencyBand[i];
            }
            audioBand[i] = frequencyBand[i] / freqBandHighest[i];
            audioBandBuffer[i] = bandBuffer[i] / freqBandHighest[i];
        }
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
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
                average += channel == Channel.Stereo ? (samplesLeft[count] + samplesRight[count]) * (count + 1) : channel == Channel.Left ? samplesLeft[count] * (count + 1) : samplesRight[count] * (count + 1);
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