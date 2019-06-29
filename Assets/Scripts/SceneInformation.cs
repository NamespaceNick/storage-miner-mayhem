using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class SceneInformation : MonoBehaviour
{
    public bool isReadyUp;
    public bool isInitialSetup;
    public bool isTitleScreen;

    public bool IsLobby() {
        return isReadyUp || isInitialSetup || isTitleScreen;
    }

    // Sue Me
    void Update() {

        foreach (InputDevice id in InputManager.Devices)
        {
            if (isTitleScreen && id.CommandWasPressed)
            {
                GameController.RequestNextLevel();
            }
        }
        // if (isTitleScreen && InputManager.ActiveDevice.CommandWasPressed) {
            //SceneManager.LoadScene(2); // showcase change
        // }
    }
}
