using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

// capture primary device reference & transition from title screen
public class TitleScreen : MonoBehaviour
{
    void Update()
    {
        // check if player has pressed start
        if (InputManager.ActiveDevice.CommandWasPressed)
        {
            // assign controller to be main controller
            GameController.controller = InputManager.ActiveDevice;
        }
    }
}
