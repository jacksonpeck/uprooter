using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class To_Game : MonoBehaviour
{
    [SerializeField] Slider numPlayersSlider;
    public void StartGame()
    {
        // Save the number of players currently displayed on the slider
        SettingsStorage.numPlayers = (int)numPlayersSlider.value;
        // Change this later to a scene that's not my name lol
        SceneManager.LoadScene("Adam");
    }
}
