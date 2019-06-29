using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour {
    public enum Direction { Left, Right };
    public float offsetDistance;
    public Direction direction;
    public GameObject player;

    List<Renderer> points = new List<Renderer>();

    // Use this for initialization
    void Start () {
        Color assignedColor = (direction == Direction.Left) ? Color.blue : Color.red;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            points.Add(r);
            r.material.color = assignedColor;
        }
    }

    // Points the directional indicator towards the normalized vector2 `dir`
    public void IndicatorDirection(Vector2 dir)
    {
        Debug.Log("indicator given direction");
        Vector3 dirVec = new Vector3(dir.x, dir.y, 0).normalized;
        bool isEnabled = (dir != Vector2.zero);
        foreach (Renderer r in points)
        {
            r.enabled = isEnabled;
        }
        transform.position = player.transform.position + (dirVec * offsetDistance);
        transform.up = (transform.position - player.transform.position).normalized;
    }

}
