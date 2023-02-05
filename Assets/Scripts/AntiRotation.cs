using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRotation : MonoBehaviour
{
    [SerializeField] GameObject[] targets;

    private void LateUpdate()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x * -1.0f, 
                gameObject.transform.rotation.y * -1.0f, 
                gameObject.transform.rotation.z * -1.0f);
        }
    }
}
