using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class To_Setting_Paused : MonoBehaviour
{
    public void toSettings_Paused()
    {
        SceneManager.LoadScene( "Setting_Pause" );
    }
}
