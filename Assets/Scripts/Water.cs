using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float _startingAmount;

    private float _amount;
    private bool _draining = false;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _amount = _startingAmount;
    }

    private void FixedUpdate()
    {
        if (_draining)
        {
            _amount -= Time.deltaTime;
            if (_amount < 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void StartDraining()
    {
        _draining = true;
    }
}
