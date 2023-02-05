using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameUI : MonoBehaviour
{
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.GetIsGameInProgress())
        {
            Destroy(gameObject);
        }
    }
}
