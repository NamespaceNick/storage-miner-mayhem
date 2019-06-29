using System.Collections.Generic;
using UnityEngine;

class Fmt {
    public static string time(float time) {
        string output;
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int ss = (int)((time - (minutes * 60) - seconds) * 10);

        if (minutes == 0)
            output = seconds.ToString();
        else if (seconds < 10)
            output = minutes.ToString() + ":0" + seconds.ToString();
        else
            output = minutes.ToString() + ":" + seconds.ToString();

        output += "." + ss.ToString();

        return output;
    }

    public static string pluralize(int count, string word) {
        string output = count + " " + word;
        if (count != 1) { output += "s"; }
        return output;
    }
}
