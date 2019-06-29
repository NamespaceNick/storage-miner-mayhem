using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerController : MonoBehaviour
{
    public PlayerStruct player; // assigned by readyupscreen/gamecontroller
    public int playerIndex;
	GrapplingHook gHook;
    Pickaxe mPick;
    Respawnable respawn;
    Stunnable stun;
    Avatar avatar;
    Coroutine playerAssignCR;

    void Start ()
    {
		gHook = GetComponent<GrapplingHook> ();
        if (gHook == null) Debug.LogWarning("gHook is Null");
        mPick = GetComponent<Pickaxe>();
        respawn = GetComponent<Respawnable>();
        stun = GetComponent<Stunnable>();
        avatar = GetComponent<Avatar>();
        if (avatar == null) Debug.LogError("Avatar is Null");
        playerAssignCR = StartCoroutine(WaitForPlayerAssign());
    }

    void Update ()
    {
        if ((player == null) || PauseMenu.gamePaused) return;
        if (player.device.LeftTrigger.IsPressed)
        {
            Debug.Log("left trigger is pressed");
        }
        if (respawn.IsRespawning()) {
            gHook.AimHook (playerIndex + 1, Vector2.zero);
            gHook.Rappel(playerIndex + 1, Vector2.zero);
            mPick.AimPick(playerIndex + 1, Vector2.zero);
            return;
        }
		gHook.AimHook (playerIndex + 1, player.device.RightStick.Value);
        gHook.Rappel(playerIndex + 1, player.device.LeftStick.Value);
        mPick.AimPick(playerIndex + 1, player.device.LeftStick.Value);
        // allow aiming while stunned, but not tossing the pickaxe or shooting the hook
        if (stun.IsStunned()) return;
        if(player.device.RightTrigger.IsPressed) {
            gHook.ThrowHook(playerIndex + 1);
        }
        if(player.device.RightTrigger.WasReleased) {
            gHook.RetractHook(playerIndex + 1);
        }
        if(!player.device.LeftTrigger.IsPressed) {
            gHook.ActivateZip(playerIndex + 1);
        }
    }

    IEnumerator WaitForPlayerAssign()
    {
        // wait until game controller has acquired all player information
        // claim players[] entries to control gHooks
		foreach (PlayerStruct potentialPlayer in GameController.players)
		{
			if ((potentialPlayer != null) && (potentialPlayer.team == avatar.team) && (potentialPlayer.playerIndex == playerIndex) && (potentialPlayer.controller == null))
			{
				potentialPlayer.controller = this;
				player = potentialPlayer;
			}
		}
        if (player == null) yield break;
        if (avatar == null) Debug.LogError("avatar is null");
		if (GameController.numPlayersSetup <= ((avatar.team * 2) + playerIndex))
		{
            Debug.LogWarning("Insufficient number of registered players for me to acquire a device");
			yield break;
		}

		if (gHook == null)
		{
			Debug.LogWarning("gHook is null");
		}
        else if (GameController.players[playerIndex] == null)
		{
			Debug.LogWarning("player index is null");
		}
		// player = GameController.players[(avatar.team * 2) + playerIndex];
        while (!gHook.allInstantiated)
        {
            yield return null;
        }
        if (gHook == null) Debug.LogError("gHook is null");
        Debug.Log("player index: " + playerIndex);
        Debug.Log("player color: " + player.color);
		gHook.SetPlayerColor (playerIndex + 1, player.color);
		if (player != null) {
			Debug.Log ("Player acquired");
		}
        else
        {
            Debug.LogWarning("I do not have a player");
        }
	}


}
