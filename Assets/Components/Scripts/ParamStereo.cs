using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamStereo : MonoBehaviour {

    public AudioPeer audioPeer;
    public int band;
    public float sensitivity = 1f;

    public enum Channel
    {
        Stereo,
        Left,
        Right
    }
    public Channel channel = new Channel();

    public bool useBuffer;

    float stereo;
    float stereoBuffer = 0;
    float stereoBufferReturnRate;
    float returnToZero;

    void Update ()
    {
        if (channel == Channel.Stereo)
        {
            stereo = Mathf.Clamp(audioPeer.stereoBandSpread[band], -1, 1);
        }
        else if(channel == Channel.Left)
        {
            stereo = Mathf.Clamp(audioPeer.stereoBandSpread[band], -1, 0);
        }
        else if (channel == Channel.Right)
        {
            stereo = Mathf.Clamp(audioPeer.stereoBandSpread[band], 0, 1);
        }

        transform.position = new Vector3(useBuffer ? GetStereoBuffer() * sensitivity : stereo * sensitivity, transform.position.y, transform.position.z);
    }

    float GetStereoBuffer()
    {
        var diff = stereoBuffer - stereo;
        return stereoBuffer = Mathf.Lerp(diff, 0f, Time.deltaTime);
    }
}