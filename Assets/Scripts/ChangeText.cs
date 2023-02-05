using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public float num;
    public Text textDisplay;
    public GameObject slider;

    void Start()
    {
        textDisplay.text = "2 Players";
    }

    public void textChange()
    {
        num = slider.GetComponent<Slider>().value;
        textDisplay.text = "";
        textDisplay.text = num.ToString() + " Players";
    }
    }
