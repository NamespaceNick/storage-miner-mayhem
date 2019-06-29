using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;

public class PauseMenu : MonoBehaviour {

    // the device that paused in the first place has all control
    public static InputDevice pauseDevice;
    public static bool gamePaused = false;
    RectTransform pausePanel;
    GameObject[] buttons;

    Vector3 target_transform = new Vector3(0, 0, 0);
    Vector3 original_transform;
    int selection;
    int currentSceneIndex;
    AudioSource audio;

    private void Start() {
        pausePanel = GameObject.Find("Pause Menu Panel").GetComponent<RectTransform>();
        audio = GetComponent<AudioSource>();
        original_transform = pausePanel.transform.localPosition;
        selection = 0;

        // grab all the buttons
        buttons = GameObject.FindGameObjectsWithTag("Button");
        Array.Sort(buttons, delegate (GameObject l, GameObject r) {
            float left = l.transform.position.y;
            float right = r.transform.position.y;
            if (left == right) {
                return 0;
            } else if (left < right) {
                return 1;
            } else {
                return -1;
            }
        });
    }

    public void PauseGame() {
        gamePaused = true;
        audio.Play();
        selection = 0;
        DrawButtons(selection);
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        audio.Play();
        gamePaused = false;
        Time.timeScale = 1;
    }

    // Pause and resume the game in here
    private void Update() {
        // commented out for the showcase
        if (gamePaused)
        {
            pausePanel.anchoredPosition = Vector2.Lerp(pausePanel.anchoredPosition, target_transform, 0.1f);

            foreach (InputDevice pauseDevice in InputManager.Devices)
            {
                if (pauseDevice.DPad.WasPressed ||
                    Debounce.On("pmnav", pauseDevice.LeftStickY != 0, 150))
                {
                    audio.Play();

                    if (pauseDevice.DPadY < 0 || pauseDevice.LeftStickY < 0)
                    {
                        selection++;
                    }
                    else
                    {
                        selection--;
                    }

                    // bound the button count
                    selection = selection % buttons.Length;
                    if (selection < 0)
                        selection = buttons.Length - 1;

                    // update the button visual flair
                    DrawButtons(selection);
                }

                // Advance by pressing A
                if (pauseDevice.Action1.WasPressed)
                {
                    // check the selection index, and act accordingly
                    // 0: resume, 1: restart the current scene, 2: load the main menu, 3: quit
                    switch (selection)
                    {
                        case 0:
                            ResumeGame();
                            break;
                        case 1:
                            Time.timeScale = 1f;
                            gamePaused = false;
                            GameController.ResetLevel();
                            break;
                        case 2:
                            Time.timeScale = 1f;
                            gamePaused = false;
                            GameController.RequestRestartGame();
                            break;
                            /*case 3: // remove exit option ****showcase
                                Application.Quit();
                                break;*/
                    }
                }

                // alternatively, exit the paused state by hitting start again
                if (pauseDevice.CommandWasPressed || pauseDevice.Action2.WasPressed)
                {
                    ResumeGame();
                }
            }
        }
        else
        {
            pausePanel.anchoredPosition = Vector3.Lerp(pausePanel.anchoredPosition, original_transform, 0.1f);

            foreach (InputDevice pauseDevice in InputManager.Devices)
            {
            if (pauseDevice.CommandWasPressed)
            {
                PauseGame();
            }
        }
        }
    }

    // essentially copy of the main menu one
    void DrawButtons(int selected = 0) {
        for(int i = 0; i < buttons.Length; i++) {
            if (selected == i) {
                buttons[i].GetComponent<HookesBounce>().small = false;
            } else {
                buttons[i].GetComponent<HookesBounce>().small = true;
            }
        }
    }

}
