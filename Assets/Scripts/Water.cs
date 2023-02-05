using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float _startingAmount;
    [SerializeField] private Sprite[] drainSprites;

    private float _amount;
    private bool _draining = false;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _amount = _startingAmount;
    }

    private void Update()
    {
        if (_draining)
        {
            _amount -= Time.deltaTime;
            Debug.Log(_amount);
            if (_amount < 0f)
            {
                Destroy(this.gameObject);
            }

            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        float progress = 1 - (_amount / _startingAmount);
        // Multi by 5 and round down, resulting in a # 0-4
        int scaledProgress = Mathf.Clamp((int)(progress * drainSprites.Length), 0, drainSprites.Length - 1);
        GetComponent<SpriteRenderer>().sprite = drainSprites[scaledProgress];
    }

    public void StartDraining()
    {
        _draining = true;
    }
}
