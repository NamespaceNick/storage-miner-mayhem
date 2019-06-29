using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

    Text team1;
    Text team2;

    float last1;
    float last2;

    // Use this for initialization
    void Start () {
        team1 = transform.Find("Team1Score").GetComponent<Text>();
        team2 = transform.Find("Team2Score").GetComponent<Text>();
        last1 = Settings.Get("Team1Score");
        last2 = Settings.Get("Team2Score");
    }

    // Update is called once per frame
    void Update () {
        float score1 = Settings.Get("Team1Score");
        float score2 = Settings.Get("Team2Score");

        team1.text = "Team 1: " + score1.ToString();
        team2.text = "Team 2: " + score2.ToString();

        if (score1 != last1) {
            team1.GetComponent<HookesBounce>().Strum();
        }

        if (score2 != last2) {
            team2.GetComponent<HookesBounce>().Strum();
        }

        last1 = score1;
        last2 = score2;
    }
}
