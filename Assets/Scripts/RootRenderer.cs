using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootRenderer : MonoBehaviour
{
    private SpriteRenderer _renderer;

    [SerializeField] private Sprite[] _rootSprites;

    private int _spriteIndex = -1;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    public void UpdateSprite(int left, int right, int up, int down)
    {
        if (left <= 0 && right <= 0 && up <= 0 && down <= 0)
        {
            _renderer.enabled = false;
            return;
        }

        if (left != right || right != up || up != down || down != left)
        {
            _renderer.enabled = false;
            Debug.Log("Error: A cell cannot contain a root for multiple players");
            return;
        }

        int rootCount = 0;

        if (left > 0) rootCount++;
        if (right > 0) rootCount++;
        if (up > 0) rootCount++;
        if (down > 0) rootCount++;

        if (_spriteIndex < 0)
            _renderer.enabled = true;

        int spriteIndex;

        switch (rootCount)
        {
        case 1:
            if (down > 0)
            {
                spriteIndex = (down - 1) * 5;
                this.transform.rotation = Quaternion.identity;
            }
            else if (right > 0)
            {
                spriteIndex = (right - 1) * 5;
                this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else if (up > 0)
            {
                spriteIndex = (up - 1) * 5;
                this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else
            {
                spriteIndex = (left - 1) * 5;
                this.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            }
            break;

        case 2:
            if (down > 0)
            {
                if (up > 0)
                {
                    spriteIndex = (left - 1) * 5 + 1;
                    this.transform.rotation = Quaternion.identity;
                }
                else
                {
                    spriteIndex = (left - 1) * 5 + 2;
                    if (right > 0) this.transform.rotation = Quaternion.identity;
                    if (left > 0) this.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                }
            }
            else if (right > 0)
            {
                if (left > 0)
                {
                    spriteIndex = (left - 1) * 5 + 1;
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                }
                else
                {
                    spriteIndex = (left - 1) * 5 + 2;
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                }
            }
            else
            {
                spriteIndex = (left - 1) * 5 + 2;
                this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            break;

        case 3:
            if (left <= 0)
            {
                spriteIndex = (right - 1) * 5 + 3;
                this.transform.rotation = Quaternion.identity;
            }
            else
            {
                spriteIndex = (left - 1) * 5 + 3;
                if (down <= 0) this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                if (right <= 0) this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                else this.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            }
            break;

        case 4:
            spriteIndex = (left - 1) * 5 + 4;
            break;

        default:
            spriteIndex = 0;
            Debug.Log("Error: I fucked up");
            break;
        }

        if (spriteIndex != _spriteIndex)
        {
            _renderer.sprite = _rootSprites[spriteIndex];
            _spriteIndex = spriteIndex;
        }
    }
}
