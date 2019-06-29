using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryController : MonoBehaviour {

    public GameObject confetti;

    bool winner1 = false;
    bool winner2 = false;
    int winner = -1;
    Text text;
    Text team1;
    Text team2;

    Vector3 flipped = new Vector3(-1f, 1f, 1f);

    void Start () {
        float score1 = Settings.Get("Team1Time");
        float score2 = Settings.Get("Team2Time");
        float diff = Mathf.Abs(score1 - score2);

        text = GameObject.Find("WinText").GetComponent<Text>();
        team1 = GameObject.Find("Team1Stats").GetComponent<Text>();
        team2 = GameObject.Find("Team2Stats").GetComponent<Text>();

        team1.text = Fmt.time(Settings.Get("Team1Time")) + "\n" +
            Fmt.pluralize((int)Settings.Get("Team1Deaths"), " death");
        team2.text = Fmt.time(Settings.Get("Team2Time")) + "\n" +
            Fmt.pluralize((int)Settings.Get("Team2Deaths"), " death");

        if (diff < 0.5f) {
            text.text = "Both Teams Win!";
        } else if (score1 < score2) {
            winner = 1;
            text.text = "Team 1 Wins!";
        } else if (score2 < score1) {
            winner = 2;
            text.text = "Team 2 Wins!";
        } else {
            text.text = "Both Teams Win!";
        }

        if (winner == -1 || winner == 1) {
            Instantiate(confetti, new Vector3(-3f, 15f, 0), Quaternion.identity);
        } else if (winner == -1 || winner == 2) {
            Instantiate(confetti, new Vector3(3f, 15f, 0), Quaternion.identity);
        }

        if (winner == 2) {
            GameObject tiles = GameObject.Find("Grid/Tilemap");
            tiles.transform.localScale = flipped;
        }

        Settings.Reset();
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame() {
        yield return new WaitForSeconds(10f);
        Settings.Set("Team1Score", 0);
        Settings.Set("Team2Score", 0);
        GameController.RequestNextLevel();
        Settings.Set("Team1Time", 0f);
        Settings.Set("Team2Time", 0f);
        Settings.Set("Team1Deaths", 0);
        Settings.Set("Team2Deaths", 0);
    }
}
