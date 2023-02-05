using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class To_HTP_Pause : MonoBehaviour
{
    public void toHTP_Pause()
    {
        SceneManager.LoadScene( "HTP_Pause" );
    }
}
