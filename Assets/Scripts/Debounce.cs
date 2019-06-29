using System.Collections.Generic;
using UnityEngine;

// Prevent triggers from being called too many times due to conditions being
// checked in several frames.
//
//   if (Debounce.On("input", Input.anyKey)) {
//       Instantiate(prefab);
//   }
//
// Ensures the prefab will only be created once every 0.5 seconds.
public class Debounce {
    static Dictionary<string, float> timers = new Dictionary<string, float>();

    static public bool On(string name, bool val, float delay = 0.5f) {
        if (timers.ContainsKey(name) && (Time.frameCount - timers[name]) < delay) {
            return false;
        }
        if (val) {
            timers[name] = Time.frameCount;
            return true;
        }
        return false;
    }
}
