using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class To_Credits_Paused : MonoBehaviour
{
    public void toCredits_Paused()
    {
        SceneManager.LoadScene( "Credits_Paused" );
    }
}
