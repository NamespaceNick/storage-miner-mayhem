using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Avatar : MonoBehaviour
{
    public int team = -1; // 0 or 1
    public float hookVisualOffset;
    public float restingHookVisualOffset;
    public PlayerStruct[] players = new PlayerStruct[2];
    public Color color;

    void Start()
    {
        Debug.Assert((gameObject.name == "Avatar 1") || (gameObject.name == "Avatar 2"));
        team = (gameObject.name == "Avatar 1") ? 0 : 1;
    }
}
