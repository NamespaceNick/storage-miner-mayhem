using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Global access to player configuration
// Used to select a level and assign number of players and controllers
//
// Only one is created in the main menu, then kept alive via DDOL.
public class Settings : MonoBehaviour {
    static Settings instance;
    Dictionary<string, float> values = new Dictionary<string, float>();

    // Use this for initialization
    void Awake () {
        if (instance != this && instance != null) {
            Destroy(gameObject);
            return;
        }
        values = new Dictionary<string, float>();
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public static void Reset() {
        instance.values.Clear();
    }

    public static void Set(string opt, float val) {
        instance.values[opt] = val;
    }

    public static void Set(string opt, bool val) {
        instance.values[opt] = (val ? 1 : 0);
    }

    public static float Get(string opt) {
        if (!instance.values.ContainsKey(opt))
            instance.values[opt] = 0;
        return instance.values[opt];
    }

    public static float Increment(string opt) {
        if (!instance.values.ContainsKey(opt))
            instance.values[opt] = 0;
        return ++instance.values[opt];
    }
}
