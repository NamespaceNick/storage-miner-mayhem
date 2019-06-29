using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceController : MonoBehaviour {
    /*
    Purposes of RaceController
        - Records times for teams
        - Records when the player wins
        - requests next level to the game controller
        - shows win text
     */
    public GameObject textPrefab;
    public GameObject timer;
    public Color yellow;
    public Color blue;
    public Color red;

    static RaceController instance;
    AudioSource startSounds;
    AudioSource music;
    HookesBounce bounce;
    Text text;
    float startTime = 0;
    float winnerTime;
    float loserTime;
    int winner = -1;

    GameObject timer1;
    GameObject timer2;

    bool inputEnabled = false;

    System.Type[] components = new System.Type[] { typeof(GrapplingHook), typeof(Pickaxe) };

    public static void PlayerHasWon(int team) {
        instance.PlayerWon(team);
    }

    void PlayerWon(int team) {
        if (winner == -1) {
            winner = team;
            winnerTime = Time.time;
        } else {
            if (team != winner) {
                loserTime = Time.time;
            }
        }
    }

    void Awake() {
        // Taking a different approach on this one
        if (instance != this) {
            Destroy(instance);
            instance = this;
        }
    }

    bool finished = false;

    void Finish() {
        if (finished) return;
        finished = true;
        RecordScores();
        GameController.RequestNextLevel();
    }

    void Update() {
        if (winner != -1) {
            text.fontSize = 100;
            text.enabled = true;
            int timeLeft = (int)Mathf.Max(Mathf.Ceil((winnerTime + 15) - Time.time), 0);
            if (timeLeft < 6 && text.text != timeLeft.ToString())
                bounce.Strum();
            text.text = timeLeft.ToString();

            if (FatLadySang()) {
                if (loserTime == 0)
                    loserTime = winnerTime + 15;
                // It's over
                Finish();
            }
        }
    }

    bool FatLadySang() {
        return (loserTime > 0) || (Time.time > winnerTime + 15);
    }

    void RecordScores() {
        string win,lose;

        if (winner == 0) {
            win = "Team1Time";
            lose = "Team2Time";
        } else {
            win = "Team2Time";
            lose = "Team1Time";
        }

        float time = Settings.Get(win);
        time += (winnerTime - startTime);
        Settings.Set(win, time);

        time = Settings.Get(lose);
        time += (loserTime - startTime);
        Settings.Set(lose, time);
    }

    void Start () {
        AudioSource[] sources = GetComponents<AudioSource>();
        startSounds = sources[0];
        music = sources[1];
        music.volume = 0;
        GameObject textobj = Instantiate(textPrefab);
        text = textobj.GetComponentInChildren<Text>();
        bounce = textobj.GetComponentInChildren<HookesBounce>();
        PrepareRace();
    }

    void PrepareRace() {
        Debug.Log("disabling input for race start");
        inputEnabled = false;
        UpdateInputComponents();
        StartCoroutine(CountDown());
    }

    void StartRace() {
        inputEnabled = true;
        UpdateInputComponents();
        // Start timer right after player controls are re-enabled
        startTime = Time.time;
        StartCoroutine(FadeInMusic());
    }

    void UpdateInputComponents() {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            //player.GetComponent<PlayerController>().enabled = inputEnabled;
            foreach (PlayerController pController in player.GetComponents<PlayerController>()) {
                pController.enabled = inputEnabled;
            }
            player.GetComponent<GrapplingHook>().enabled = inputEnabled;
            player.GetComponent<Pickaxe>().enabled = inputEnabled;
        }
    }

    IEnumerator FadeInMusic() {
        while (music.volume != 1.0f) {
            music.volume += 0.01f;
            yield return null;
        }
    }

    public IEnumerator FadeOutMusic() {
        while (music.volume != 0f) {
            music.volume -= 0.05f;
            yield return null;
        }
    }

    IEnumerator CountDown() {
        yield return new WaitForSeconds(2f);
        startSounds.Play();
        float time = 0;
        int itime = 0;
        text.text = "3";
        text.fontSize = 200;
        while (time < 3f) {
            if ((3 - (int)time) != itime) {
                itime = (3 - (int)time);
                bounce.Strum();
            }
            text.text = (3 - (int)time).ToString();
            time += Time.deltaTime;
            yield return null;
        }
        bounce.Strum();
        text.text = "Go!";
        StartRace();
        CreateTimers();
        yield return new WaitForSeconds(1f);
        text.text = "";
        text.color = red;
    }

    void CreateTimers() {
        timer1 = Instantiate(timer);
        timer1.GetComponentInChildren<Text>().color = yellow;

        timer2 = Instantiate(timer);
        timer2.GetComponentInChildren<Text>().color = blue;
        RectTransform trans = timer2.transform.Find("Text").gameObject.GetComponent<RectTransform>();
        trans.anchorMax = Vector2.one;
        trans.anchorMin = Vector2.one;
        trans.anchoredPosition = new Vector2(-75, trans.anchoredPosition.y);

        StartCoroutine(UpdateTimers());
    }

    void UpdateTimer(Text text, int team, float time) {
        float ntime = time - startTime;
        ntime += Settings.Get(team == 1 ? "Team1Time" : "Team2Time");
        text.text = Fmt.time(ntime);
    }

    IEnumerator UpdateTimers() {
        while (true) {
            if (finished) { break; }
            Text t1 = timer1.GetComponentInChildren<Text>();
            Text t2 = timer2.GetComponentInChildren<Text>();

            if (winner == -1) {
                UpdateTimer(t1, 1, Time.time);
                UpdateTimer(t2, 2, Time.time);
            } else {
                float lose = (loserTime != 0) ? loserTime : Time.time;

                if (winner == 0) {
                    UpdateTimer(t1, 1, winnerTime);
                    UpdateTimer(t2, 2, lose);
                } else {
                    UpdateTimer(t1, 1, lose);
                    UpdateTimer(t2, 2, winnerTime);
                }
            }

            yield return null;
        }
    }
}
