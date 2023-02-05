using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings_Transition : MonoBehaviour
{
    public void NewScene()
    {
        SceneManager.LoadScene( "Settings_Menu" );
    }
}
