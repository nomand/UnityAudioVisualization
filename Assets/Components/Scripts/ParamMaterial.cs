using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamMaterial : MonoBehaviour
{
    public enum ValueType
    {
        Color,
        Float
    }

    public AudioPeer audioPeer;
    public int band;
    public bool useBuffer;

    public ValueType valueType = new ValueType();

    public float sensitivity;

    public string ShaderProperty = "_EmissionColor";

    Material material;

	void Start ()
    {
        material = GetComponent<MeshRenderer>().materials[0];

        switch(valueType)
        {
            case ValueType.Color: valueType = ValueType.Color; break;
            case ValueType.Float: valueType = ValueType.Float; break;
        }
	}
	
	void Update ()
    {
        var value = useBuffer ? audioPeer.AudioBandBuffer[band] : audioPeer.AudioBand[band];
        value *= sensitivity;

        Color color = new Color(value, value, value);

        if(valueType == ValueType.Color)
        {
            material.SetColor(ShaderProperty, color);
        }
        if(valueType == ValueType.Float)
        {
            material.SetFloat(ShaderProperty, value);
        }
    }
}