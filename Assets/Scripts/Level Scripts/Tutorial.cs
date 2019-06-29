using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {
    /*
    public Canvas hintUI;
    public Text hintText;
    InputDevice activeInput;
    PlayerController playerOne;
    PlayerController playerTwo;
    GameObject[] players;
    // other scenes can reference tutorialDone to figure out
    // whether to start the tutorial when the player dies or not
    public static bool tutorialDone = false;

    // Use this for initialization
    void Start () {
        // grab player objects
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0 || players.Length > 2) {
            // Debug.Log("Impossible number of players: " + players.Length + " :(");
        } else {
            // might be moot if we continue to spawn two players every time we enter a scene
            // Debug.Log("Player count:" + players.Length);
            playerOne = players[0].GetComponent<PlayerController>();
            if (players.Length == 2) {
                playerTwo = players[1].GetComponent<PlayerController>();
            }
        }


        if (!tutorialDone) {
            StartCoroutine(WaitTutorialEnable());
        } else {
            hintUI.enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {
        // skip tutorial button
        // if pressed, take the players to the introductory level
    }

    // ugly ass sphagetti condition checks incoming
    // switch statement that checks for various stages of required input
    bool TutorialPlayerInput(int stage) {
        switch(stage) {
            case 1:
                // check for any stick input from player one (and player two, if they exist)
                // Debug.Log("CHECKING FOR STICK INPUT");
                // due to the way we assign controllers, playerOne.leftDevice always exists
                if (playerOne.leftDevice.LeftStick.Vector != Vector2.zero ||
                    playerOne.leftDevice.RightStick.Vector != Vector2.zero ||
                    (playerOne.rightDevice != null &&
                    playerOne.rightDevice.RightStick.Vector != Vector2.zero) ||
                    (playerTwo.leftDevice != null && (
                    playerTwo.leftDevice.LeftStick.Vector != Vector2.zero ||
                    playerTwo.leftDevice.RightStick.Vector != Vector2.zero ||
                    (playerTwo.rightDevice != null &&
                    playerTwo.rightDevice.RightStick.Vector != Vector2.zero)))) {
                    return true;
                } else return false;
            case 2:
                // Debug.Log("CHECKING FOR ACTIVE HOOK INPUT");
                if (playerOne.leftDevice.LeftTrigger.WasPressed ||
                    playerOne.leftDevice.RightTrigger.WasPressed ||
                    (playerOne.rightDevice != null &&
                    playerOne.rightDevice.RightTrigger.WasPressed) ||
                    (playerTwo.leftDevice != null && (
                    playerTwo.leftDevice.LeftTrigger.WasPressed ||
                    playerTwo.leftDevice.RightTrigger.WasPressed ||
                    (playerTwo.rightDevice != null &&
                    playerTwo.rightDevice.RightTrigger.WasPressed)))) {
                    return true;
                } else return false;
            case 3:
                // Debug.Log("CHECKING FOR PASSIVE HOOK INPUT");
                if (playerOne.leftDevice.LeftBumper.WasPressed ||
                    playerOne.leftDevice.RightBumper.WasPressed ||
                    (playerOne.rightDevice != null &&
                    playerOne.rightDevice.RightBumper.WasPressed) ||
                    (playerTwo.leftDevice != null && (
                    playerTwo.leftDevice.LeftBumper.WasPressed ||
                    playerTwo.leftDevice.RightBumper.WasPressed ||
                    (playerTwo.rightDevice != null &&
                    playerTwo.rightDevice.RightBumper.WasPressed)))) {
                    return true;
                } else return false;
        }

        return true;
    }

    IEnumerator WaitTutorialEnable() {
        while (!GameController.playersAssigned) {
            yield return null;
        }
        StartCoroutine(TutorialRoutine());
        yield return null;
    }

    IEnumerator TutorialRoutine() {
        bool tutorialStagePass;
        hintUI.enabled = true;
        hintText.text = "Hello monkey.";
        yield return new WaitForSecondsRealtime(2f);
        hintText.text = "I will follow your progress today.";
        yield return new WaitForSecondsRealtime(2f);
        hintText.text = "Make it to the end, and you get an extra banana for dinner.";
        yield return new WaitForSecondsRealtime(2f);

        hintText.text = "Point your arms with the left and right analog sticks.";
        // wait for input from left/right stick of any character
        tutorialStagePass = TutorialPlayerInput(1);
        while (!tutorialStagePass) {
            tutorialStagePass = TutorialPlayerInput(1);
            yield return null;
        }
        hintText.text = "Good.";
        yield return new WaitForSecondsRealtime(2f);

        hintText.text = "Grab the square with your fast hooks. Pull the trigger buttons.";
        // wait for input from left/right stick of any character
        tutorialStagePass = TutorialPlayerInput(2);
        while (!tutorialStagePass) {
            tutorialStagePass = TutorialPlayerInput(2);
            yield return null;
        }
        hintText.text = "Good.";
        yield return new WaitForSecondsRealtime(2f);

        hintText.text = "Grab the square with your slow hooks. Pull the bumper buttons.";
        // wait for input from left/right stick of any character
        tutorialStagePass = TutorialPlayerInput(3);
        while (!tutorialStagePass) {
            tutorialStagePass = TutorialPlayerInput(3);
            yield return null;
        }
        hintText.text = "Good.";
        yield return new WaitForSecondsRealtime(2f);

        hintText.text = "That's all you need to know, monkey.";
        yield return new WaitForSecondsRealtime(2f);

        while (!tutorialDone) {
            hintText.text = "Get to the finish!";
            yield return null;
        }

        hintText.text = "Well done, monkey! One more banana for you tonight.";
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene(2);
        yield return null;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            tutorialDone = true;
        }
    }
    */
}
