using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using InControl;

// Game Controller for the main menu
public class MainMenu : MonoBehaviour {
    GameObject title;

    enum Stage { Title, Levels, Players, Done };
    Stage current = Stage.Title;
    int selection = 0;
    List<GameObject> stages;
    Camera cam;
    GameObject[] btns;
    GameObject[] buttons;

    void Start() {
        cam = Camera.main;

        // Load all the possible stages as top-level children of the canvas.
        // Each stage is contained inside one empty element.
        stages = new List<GameObject>();
        stages.Add(GameObject.Find("/Canvas/Title"));
        stages.Add(GameObject.Find("/Canvas/Levels"));
        stages.Add(GameObject.Find("/Canvas/Players"));

        // Collect all buttons, then narrow by stage.
        btns = GameObject.FindGameObjectsWithTag("Button");
        SetButtons();
    }

    // Set buttons to the set of elements currently on the screen
    void SetButtons() {
        if (current == Stage.Done) { return; }

        buttons = Array.FindAll(btns, btn =>
                                btn.transform.parent == stages[(int)current].transform);

        // Sort the buttons from top to bottom (y-axis)
        // If someone can find a better way to sort this, LMK
        Array.Sort(buttons, delegate(GameObject l, GameObject r) {
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

    void Update() {
        // Make sure we're only looking at the current stage.
        for (int i = 0; i < stages.Count; i++) {
            if (i == (int)current) {
                stages[i].SetActive(true);
            } else {
                stages[i].SetActive(false);
            }
        }

        // Allow selection of elements within a stage.
        // CHANGE: Swapped to D-pad controls instead of stick - more precise
        /*if (Debounce.On("select", InputManager.ActiveDevice.LeftStick.Y != 0)) {
            bool inc = InputManager.ActiveDevice.LeftStick.Y < 0; // Down is up

            // Move the cursor up and down bounded by button count
            if (inc)
                selection++;
            else
                selection--;
            selection = selection % buttons.Length;
            if (selection < 0)
                selection = buttons.Length - 1;

            // Draw the buttons
            DrawButtons(selection);

            // Update the stored data
            UpdateSettings();
        }*/

        if (InputManager.ActiveDevice.DPad.WasPressed) {

            // Move the cursor up and down bounded by button count
            if (InputManager.ActiveDevice.DPadY < 0)
                selection++;
            else
                selection--;
            selection = selection % buttons.Length;
            if (selection < 0)
                selection = buttons.Length - 1;

            // Draw the buttons
            DrawButtons(selection);

            // Update the stored data
            UpdateSettings();
        }

        // Advance by pressing A
        if (InputManager.ActiveDevice.Action1.WasPressed) {
            current = (Stage)((int)current + 1);
            SwitchStageSelection(current);
            SetButtons();
            DrawButtons();
            UpdateSettings();
        // Go back by pressing B
        } else if (InputManager.ActiveDevice.Action2.WasPressed
                    && current != Stage.Title) {
            // Prevent a player pressing B to exit out of the Stage array
            current = (Stage)((int)current - 1);
            SwitchStageSelection(current);
            SetButtons();
            DrawButtons();
            UpdateSettings();
        }

        // TODO: Add step back on B/O

        // If we've advanced to the end, load the correct scene
        if (current == Stage.Done) {
            StartCoroutine(LoadNextScene());
        }
    }

    void SwitchStageSelection(Stage currentStage) {
        switch (current) {
            case Stage.Levels:
                selection = (int)Settings.Get("Level");
                break;
            case Stage.Players:
                selection = (int)Settings.Get("PlayerCount") - 1;
                if (selection == -1)
                    selection = 0;
                if (selection == 3)
                    selection = 2;
                break;
        }
    }

    void UpdateSettings() {
        // Store the selection result based on the stage
        switch (current) {
            case Stage.Levels:
                Settings.Set("Level", selection);
                break;

            case Stage.Players:
                int count = selection;
                if (count == 2)
                    count = 3;
                Debug.Log("PlayerCount: " + (count + 1).ToString());
                Settings.Set("PlayerCount", count + 1);
                break;
        }
    }

    // Make the buttons grow or shrink based on if they're selected
    void DrawButtons(int selected = 0) {
        for (int i = 0; i < buttons.Length; i++) {
            if (selected == i) {
                buttons[i].GetComponent<HookesBounce>().small = false;
            } else {
                buttons[i].GetComponent<HookesBounce>().small = true;
            }
        }
    }

    // Fade to black and load the selected scene
    IEnumerator LoadNextScene() {
        while (cam.backgroundColor != Color.black) {
            cam.backgroundColor = Color.Lerp(cam.backgroundColor, Color.black, 0.01f);
            yield return new WaitForSeconds(0.05f);
        }

        SceneManager.LoadScene((int)Settings.Get("Level") + 1);
    }
}
