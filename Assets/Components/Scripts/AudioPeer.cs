using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource audioSource;

    public AnimationCurve FrequencyDistributionCurve;
    float[] frequencyDistribution = new float[512];

    [HideInInspector]
    public float[] AudioBand, AudioBandBuffer;
    [HideInInspector]
    public float Amplitude, AmplitudeBuffer;
    
    private float[] samplesLeft = new float[512];
    private float[] samplesRight = new float[512];

    public float[] frequencyBand;
    private float[] bandBuffer;
    private float[] bufferDecrease;
    private float[] freqBandHighest;

    private float amplitudeHighest;

    [Range(0, 512)]
    public int FrequencyBands = 8;
    public float AudioProfile;

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

        frequencyBand = new float[FrequencyBands];
        bandBuffer = new float[FrequencyBands];
        bufferDecrease = new float[FrequencyBands];
        freqBandHighest = new float[FrequencyBands];

        AudioBand = new float[FrequencyBands];
        AudioBandBuffer = new float[FrequencyBands];

        GetFrequencyDistribution();
        MakeAudioProfile(AudioProfile);
    }

    void Update ()
    {
        GetSpectrumAudioSource();

        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();

        //for(int i = 0; i < FrequencyBands; i++)
        //{
        //    print(frequencyBand[i]);
        //}
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
    }

    void GetFrequencyDistribution()
    {
        for(int i = 0; i < FrequencyBands; i++)
        {
            var eval = ((float)i) / FrequencyBands;
            frequencyDistribution[i] = FrequencyDistributionCurve.Evaluate(eval);
        }
    }

    void MakeFrequencyBands()
    {
        int band = 0;
        float average = 0;

        for (int i = 0; i < 512; i++)
        {
            var sample = (float)i;
            var current = FrequencyDistributionCurve.Evaluate(sample / 512);

            if(channel == Channel.Stereo)
                average += (samplesLeft[i] + samplesRight[i]) * (i + 1);
            else if(channel == Channel.Left)
                average += samplesLeft[i] * (i + 1);
            else if(channel == Channel.Right)
                average += samplesRight[i] * (i + 1);

            if (current >= frequencyDistribution[band])
            {
                if (i != 0) { average /= i; }
                frequencyBand[band] = average * 10;
                band++;
            }
        }
    }

    void BandBuffer()
    {
        for(int i = 0; i < FrequencyBands; i++)
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
        for (int i = 0; i < FrequencyBands; i++)
        {
            if (frequencyBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = frequencyBand[i];
            }

            AudioBand[i] = Mathf.Clamp01(frequencyBand[i] / freqBandHighest[i]);
            AudioBandBuffer[i] = Mathf.Clamp01(bandBuffer[i] / freqBandHighest[i]);
        }
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for(int i = 0; i < FrequencyBands; i++)
        {
            currentAmplitude += AudioBand[i];
            currentAmplitudeBuffer += AudioBandBuffer[i];
        }
        if(currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }
        Amplitude = currentAmplitude / amplitudeHighest;
        AmplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }

    void MakeAudioProfile(float audioProfile)
    {
        for(int i = 0; i < FrequencyBands; i++)
        {
            freqBandHighest[i] = audioProfile;
        }
    }
}