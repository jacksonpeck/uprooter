using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerIs : MonoBehaviour
{
    public Sprite spriteRed;
    public Sprite spriteGreen;
    public Sprite spriteBlue;
    public Sprite spritePurple;

    private float playerNum;
    public new SpriteRenderer renderer;
    private Sprite newSprite;

    void Start()
    {
        playerNum = GameManager.Instance.connectedPlayers[0].playerNum;
        switch( playerNum )
        {
            case 1:
                newSprite = spriteGreen; 
                break;
            case 3:
                newSprite = spriteBlue; 
                break;
            case 4:
                newSprite = spritePurple; 
                break;
            case 2:
                newSprite = spriteRed;
                break;
            default:
                newSprite = spriteRed;
                break;
        }

        renderer.sprite = newSprite;
        renderer.transform.localScale = new Vector3( 20, 20, 0 );
    }
}
