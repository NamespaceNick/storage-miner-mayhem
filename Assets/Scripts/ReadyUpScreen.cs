using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;


public class ReadyUpScreen : MonoBehaviour
{
    /*
    purposes of ReadyUpScreen
        - starts initial setup coroutine (devices)
        - coordinates players to the icons [remove]
        - moves icons and controls player teams [remove]
        - acquires initial device references for 4 players [remove]
        - categorizes players by team [remove]
        - handles countdown after team select for the game to start [remove]
     */

    public Color[] playerColors = new Color[4];
    public GameObject[] playerIcons = new GameObject[4];
    public int numPlayersSetup = 0;
    public int numPlayersPlaying = 4;
    public bool testEasySetup = false;
    public bool showcaseTeams = false;
    public float iconLerp = 0.1f;
    public float iconDistance = 2f;
    public float stickBuffer = 0.7f; // [0,1]

    bool teamsSetup = false;
    bool countingDown = false;
    bool forceStart = false;
    public bool startedGame = false;
    Text initialSetupText;
    Text readyUpText;

    Coroutine initialSetupCR, readyUpCR, playerIconCR, countdownCR;
    SceneInformation sceneInfo;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneInfo = GameObject.Find("Scene Information").GetComponent<SceneInformation>();
        if (sceneInfo.isInitialSetup)
        {
            //Debug.Log("Beginning initial setup");
            // TODO: these variables aren't used anywhere
            initialSetupCR = StartCoroutine(InitialSetup());
            playerIconCR = StartCoroutine(HandlePlayerIcons());
            // StartCoroutine (CountDown ());
        }
        else if (sceneInfo.isReadyUp)
        {
            // Debug.Log("Beginning ready up");
            readyUpCR = StartCoroutine(ReadyUp());
        }
    }

    void Update() {
        if (sceneInfo && sceneInfo.isInitialSetup) {
            // comment this out if soft lock isnt fixed
            bool lostIcons = false;
            foreach (GameObject icon in playerIcons) {
                if (icon == null) {
                    lostIcons = true;
                    break;
                }
            }
            if (lostIcons) {
                Debug.Log("lost icons in ready up script!");
                // checked the ready up screen - the icons are the only ones with the player tag, so hopefully this works
                playerIcons = GameObject.FindGameObjectsWithTag("Player");
                // enabling these breaks controller detection.
                //StartCoroutine(InitialSetup());
                //StartCoroutine(HandlePlayerIcons());
            }
            // end of block to comment out for soft lock
            TeamSetupUpdate();
        }
    }

    void TeamSetupUpdate() {
        // Always lerp the icon to the player's selection. This prevents delays
        // due to animation speed and feels more responsive.
        foreach (PlayerStruct player in GameController.players) {
            if (player == null || player.team == -1) { continue; }

            Vector3 destination = Vector3.zero;
			// TODO: This is another instance that is contributing to the 
			// soft lock, cannot find the player icons
            destination.x = playerIcons[player.iconIndex].transform.position.x;
            destination.y = iconDistance;

            if (player.team == 1) { destination.y *= -1; }

            playerIcons[player.iconIndex].transform.position =
                Vector3.Lerp(playerIcons[player.iconIndex].transform.position, destination, iconLerp);
        }
    }

    // stores and assigns all 4 device references
    IEnumerator InitialSetup()
    {
        // Debug.Log ("in initial setup");
        // disable icon rendering before players have pressed start
        foreach (GameObject icon in playerIcons)
        {
			// TODO: This contributes to the soft lock, need to use 
			// GameObject.Find() or something, so that the icons are still able 
			// to be acquired
            icon.GetComponent<SpriteRenderer>().enabled = false;
        }
        int playerIconIndex = 0;
        numPlayersSetup = 0;
        initialSetupText = GameObject.Find("Initial Setup Text").GetComponent<Text>();
        string initialText = initialSetupText.text;
        // while there are still players that need to press start
        // FIXME: I think a lot of this might be redundant because of the HandleIcons() coroutine, remove redundancies to ensure
        // FIXME: force start mode still works and other features aren't scuffed
        Debug.Log("entering setup loop");
        while ((numPlayersSetup < numPlayersPlaying) && !forceStart)
        {
            // Debug.Log ("in setup loop");
            // iterate through each stored device
            foreach (InputDevice dev in InputManager.Devices)
            {
                Debug.Log(dev.Name);

                // premature start mode (press B on xbox controller, circle on dualshock 4)
                if (dev.Action2.WasPressed && (numPlayersSetup > 0) && testEasySetup)
                {
                    Time.timeScale = 1f;
                    Debug.Log("Developer mode easy setup enabled");
                    for (int i = 0; i < numPlayersSetup; ++i)
                    {
                        GameController.players[i].color = playerColors[i];
                    }
                    GameController.numPlayersSetup = numPlayersSetup;
                    GameController.playersAssigned = true;
                    initialSetupText.enabled = false;
                    Time.timeScale = 1f;
                    Debug.Log ("Calling from force start");
                    forceStart = true;
                    GoToReadyUp();
                }
                // instantly grab the device ****showcase
                if (!(dev.CommandWasPressed || dev.AnyButtonWasPressed) && !showcaseTeams) continue;
                bool alreadySetup = false;
                // make sure the controller isn't in use by another player
                for (int i = 0; i < numPlayersSetup; ++i)
                {
                    alreadySetup |= (dev == GameController.players[i].device);
                }
                if (!alreadySetup)
                {
                    // setup controller
                    playerIcons[playerIconIndex].GetComponent<SpriteRenderer>().enabled = true;
                    Debug.Log("Additional controller setup");
                    PlayerStruct toAdd = new PlayerStruct(dev, playerIconIndex);
                    // instantly assign team ****showcase
                    if (showcaseTeams) {
                        if (playerIconIndex < 2) toAdd.team = 0;
                        else toAdd.team = 1;
                    }
                    GameController.players[numPlayersSetup] = toAdd;
                    numPlayersSetup++;
                    playerIconIndex++;
                }
            }
            yield return null;
        }
    }

    IEnumerator ReadyUp ()
    {
        Debug.Log ("in ready up");
        while (GameController.numReady < 2)
        {
            yield return null;
            if (testEasySetup)
            {
                foreach (PlayerStruct player in GameController.players)
                {
                    if (player == null) continue;
                    if (player.device.Action2.WasPressed)
                    {
                        GameController.RequestNextLevel();
                        yield break;
                    }
                }
            }
        }
        // Text numReadyText = GameObject.Find ("Number Ready Text").GetComponent<Text> ();
        // string originalText = numReadyText.text;
        yield return new WaitForSeconds (1f);
        Debug.Log ("calling new game from ready up script");
        GameController.RequestNextLevel();
        yield break;
    }


    bool ChoseTeam(PlayerStruct player, int teamCheck)
    {
        Debug.Assert(teamCheck == 0 || teamCheck == 1);
        bool wasChosen = (teamCheck == 1) ? player.device.DPadDown.WasPressed : player.device.DPadUp.WasPressed;
        if (teamCheck == 1)
        {
            return (player.device.LeftStick.Value.y <= -stickBuffer) || (player.device.DPadDown.WasPressed);
        }
        else
        {
            return (player.device.LeftStick.Value.y >= stickBuffer) || (player.device.DPadUp.WasPressed);
        }
    }

    IEnumerator HandlePlayerIcons()
    {
        while (!startedGame)
        {
            int numSetup = 0;
            int numTeamZero = 0, numTeamOne = 0;
            yield return null;
            foreach (PlayerStruct player in GameController.players)
            {
                // top = team0, bottom = team1, middle = team-1
                // if the player hasn't pressed start yet or we're making the showcase build, dont bother checking
                if (player == null || showcaseTeams) continue; 
                if (ChoseTeam(player, 1))
                {
                    player.team = 1;
                }
                if (ChoseTeam(player, 0))
                {
                    player.team = 0;

                }
                if (player.team != -1) numSetup++;
                if (player.team == 0) numTeamZero++;
                if (player.team == 1) numTeamOne++;
            }
            // Debug.Log("Num players chosen team: " + numSetup);
            // Debug.Log("Num players on team0 (top): " + numTeamZero);
            // Debug.Log("Num players on team1 (bottom): " + numTeamOne);
            if ((numSetup == 4) && (numTeamOne == 2) && (numTeamZero == 2))
            {
                if (!countingDown && !startedGame)
                {
                    teamsSetup = true;
                    countingDown = true;
                    countdownCR = StartCoroutine (CountDown ());
                }
                // StartCoroutine (CountDown ());
            } else if (!forceStart)
            {
                teamsSetup = false;
            }
        }
    }

    // begin countdown for the ready-up screen
    // TODO: Fix why this function is called a lot
    IEnumerator CountDown()
    {
        string originalSetupText = initialSetupText.text;
        float timePassed = 0f;
        int timeLeft;
        while (teamsSetup && (timePassed < 4))
        {
            Debug.Log ("In loop");
            // if (startedGame) yield break;
            timeLeft = (int)(4f - timePassed);
            Debug.Log ("timeLeft: " + timeLeft);
            initialSetupText.text = timeLeft.ToString();
            timePassed += Time.deltaTime;
            yield return null;
        }
        if (!startedGame && teamsSetup)
        {
            startedGame = true;
            countingDown = false;
            initialSetupText.enabled = false;
            Debug.Log ("Calling next level in count down");
            GoToReadyUp();
            yield break;
        } else
        {
            if (!startedGame)
            {
                initialSetupText.text = originalSetupText;
            }
            startedGame = false;
            countingDown = false;
            yield break;
        }
    }


    void GoToReadyUp()
    {
        GameController.numPlayersSetup = numPlayersSetup;
        // initialSetupText.enabled = false;
        // all devices acquired and assigned to players

        // additional player information
        // TODO: Encapsulate in a function so it can be called on force start and normal start
        int t0Idx = 0;
        int t1Idx = 0;
        for (int i = 0; i < numPlayersSetup; ++i)
        {
            GameController.players[i].color = playerColors[i];
            GameController.players[i].playerIndex = (GameController.players[i].team == 0) ? t0Idx++ : t1Idx++;
        }

        // tell game controller to assign players to avatars and begin game
        GameController.playersAssigned = true;
        GameController.RequestNextLevel();
    }
}
