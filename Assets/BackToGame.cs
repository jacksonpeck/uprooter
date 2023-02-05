using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToGame : MonoBehaviour
{
    public void toGame()
    {
        SceneManager.LoadScene( "Adam" );
    }
}
