using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back_To_Main : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene( "Main_Menu" );
    }
}
