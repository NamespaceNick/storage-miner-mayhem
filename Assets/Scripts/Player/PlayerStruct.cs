using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerStruct 
{

	public PlayerStruct(){}
	public PlayerStruct(InputDevice dev, int iconIdx)
	{
		device = dev;
		iconIndex = iconIdx;
	}
    public InputDevice device;
    public Color color;
	public PlayerController controller;
    public int team = -1; // Can be 0 or 1
    public int iconIndex = -1;
    public int playerIndex = -1;
    public bool iconMoving = false;
}
