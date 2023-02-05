using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    public AudioMixer mixer;

    public void setLevel( float sliderValue )
    {
        mixer.SetFloat( "Vol", Mathf.Log10( sliderValue ) * 20 );
    }
}
