using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public bool initDone;

    void Start()
    {
        if(initDone == false)
             {
                 initDone = true;
                 mixer.SetFloat( "Vol", Mathf.Log10( 0.5f ) * 20 );
             }
    }
    public void setLevel( float sliderValue )
    {
        mixer.SetFloat( "Vol", Mathf.Log10( sliderValue ) * 20 );
    }
}
