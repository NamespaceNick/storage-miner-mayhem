using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

// Control game-level concerns like victory
public class GameController : MonoBehaviour {
    /*
    Current purposes of GameController
        - Control scene transitions
        - Control race music
        - clears out player devices when returned to the screen

     */
    // new variables
    public static bool singlePlayer = true;
    public static InputDevice controller;   // for single player mode


    // end of new variables

    public static PlayerStruct[] players = new PlayerStruct[4];
    public static GameController _instance;
    // FIXME: I don't think we should connect the text and the game controller
    static GameObject text;
    public static int numReady = 0;
    public static bool playersAssigned;
    public static bool playersReady;
    public static int numPlayersSetup = 0;
    public static bool performingSceneTransition = false;
    public static bool levelStarted = false;
    public bool showcaseBuild = false;
    Avatar avatar1, avatar2;
    TransitionCamera mainCam;
    int nextLevelCycle = 0;

    // Make sure the time is running properly and only assign one _instance
    void Awake()
    {
        if (_instance != this && _instance != null) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TransitionCamera>();
        mainCam.FadeCameraIn();
    }

    // Find the win text and turn it off
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(mainCam);
        playersAssigned = false;
        if (text)
            text.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
        // FIXME: If the scene is loaded, we need to look for the scene information and make our decisions based on that.
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // TODO: Countdown timer for each level
        // StartCoroutine(LevelStartTimer());
        numReady = 0;
        performingSceneTransition = false;
        Debug.Log("Getting avatars");
		if (scene.buildIndex > 0)
		{
            GameObject avObj1 = GameObject.Find("Avatar 1");
            GameObject avObj2 = GameObject.Find("Avatar 2");
            if(avObj1 != null && avObj2 != null) {
    			avatar1 = avObj1.GetComponent<Avatar> ();
    			avatar2 = avObj2.GetComponent<Avatar> ();
            }
		}
		mainCam.FadeCameraIn();
    }

    // Provide a nice function call to reset the level
    public static void ResetLevel()
    {
        Settings.Set("Team1Time", 0f);
        Settings.Set("Team2Time", 0f);
        Settings.Set("Team1Deaths", 0);
        Settings.Set("Team2Deaths", 0);
        _instance.StartCoroutine(_instance.PerformReset());
    }

    // Allow a delay in resetting
    IEnumerator PerformReset()
    {
        yield return _instance.mainCam.FadeOut();
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
    }

    public static void RequestNextLevel()
    {
        _instance.StartCoroutine(_instance.NextLevel());
    }

    IEnumerator NextLevel()
    {
        GameObject raceobj = GameObject.Find("RaceController(Clone)");
        if (raceobj) 
        {
            RaceController race = raceobj.GetComponent<RaceController>();
            StartCoroutine(race.FadeOutMusic());
        }
        yield return _instance.mainCam.FadeOut();
        if (showcaseBuild) 
        {
            Debug.Log("showcase time baby");
            int currLevel = SceneManager.GetActiveScene().buildIndex;
            if (currLevel == 6) {
                StartCoroutine(LoadSceneAsync(nextLevelCycle)); // if we're on the victory level, load the next scene in the cycle
            } else {
                if (currLevel == 5) nextLevelCycle = 3; // if we're on the brick level, set nextLevelCycle to 3 to go back to dirt land
                else nextLevelCycle = currLevel + 1;
                Debug.Log("loading:" + nextLevelCycle);
                if (currLevel < 3) StartCoroutine(LoadSceneAsync(nextLevelCycle)); // if we're not at the dirt level yet
                                                                                   // (tutorial/main menu) just load the next scene in the sequence
                else StartCoroutine(LoadSceneAsync(6)); // else,jump to the victory stage
            }

        } else {
            int next = SceneManager.GetActiveScene().buildIndex + 1;
            next = next % SceneManager.sceneCountInBuildSettings;
            StartCoroutine(LoadSceneAsync(next));
        }
    }

    public static void RequestRestartGame()
    {
        _instance.StartCoroutine(_instance.RestartGame());
    }

    IEnumerator RestartGame()
    {
		//ClearPlayers ();
        yield return _instance.mainCam.FadeOut();
        StartCoroutine(LoadSceneAsync(1)); //showcase change from 0 to 1
    }

    IEnumerator LoadSceneAsync(int sceneIndex) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone) {
            yield return null;
        }
        mainCam.FadeCameraIn();
    }

	// Called before game returns to team setup screen, so that devices
	// can be reacquired
	void ClearPlayers()
	{
		for (int i = 0; i < 4; ++i)
		{
			if (players [i] == null) continue;
			players [i] = null;
		}
		playersReady = false;
		playersAssigned = false;
		numReady = 0;
		levelStarted = false;
	}
}
