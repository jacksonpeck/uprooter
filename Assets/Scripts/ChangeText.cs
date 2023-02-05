using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public Sprite spriteTwo;
    public Sprite spriteThree;
    public Sprite spriteFour;


    public float num;
    public GameObject slider;
    private float index;
    public new SpriteRenderer renderer;
    private Sprite newSprite;

    void Start()
    {
        slider.GetComponent<Slider>().value = 2;
    }

    public void textChange()
    {
        num = slider.GetComponent<Slider>().value;
        index = num + 51;
        newSprite = null;

        switch( index )
        {
            case 53:
                newSprite = spriteTwo; 
                break;
            case 54:
                newSprite = spriteThree; 
                break;
            case 55:
                newSprite = spriteFour; 
                break;
            default:
                newSprite = spriteTwo;
                break;
        }

        renderer.sprite = newSprite;
        renderer.transform.localScale = new Vector3( 20, 20, 0 );
    }
}
