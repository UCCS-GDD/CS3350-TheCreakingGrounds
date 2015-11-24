using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using Assets.Scripts;

public class GibberingMadness : NetworkBehaviour {

    public Canvas winCanvas;
    public Canvas loseCanvas;
    public Canvas diedCanvas;

    public GameObject gibberingMadnessVia;
    public GameObject gibberingMadnessSound;

    public bool isCursed = false;

    private List<Player> playersInReach = new List<Player>();

    public static string playerBriefing = "You are a Survivor. You are trying to defeat the Betrayer.\n" +
        "In this Curse, the Betrayer is the Gibbering Madness, your friend is possessed by an insane spirit hellbent on killing you and the others. " +
        "When the Gibbering Madness is near you it will give you Traumas. " +
        "However, the more players you are near the more likely it is that you give a Wound back onto the Gibbering Madness.\n\n" +
        "If you receive too many Traumas, you die. If the Betrayer receives too many Wounds, they die.\n\n" +
        "Use WILLPOWER to defend and avoid taking traumas.\n" +
        "Use BRAWN to attack and inflict wounds.\n\n" +
        "Focus on grouping up to defeat the Gibbering Madness.\n\n" +
        "Kill the Gibbering Madness while at least one Survivor remains to win.";

    public static string curseBriefing = "\tYou are the Betrayer. You are trying to kill the party.\n" +
        "In this Curse, you are the Gibbering Madness, a body possessed by the insane spirit that resides within this mansion. " +
        "By being near the players, you give them Traumas, however, the more players you are trying to affect, the more likely it is that you receive Wounds in return.\n\n" +
        "If you receive too many Wounds, you die. If they receive too many Traumas, they die.\n\n" +
        "Use WILLPOWER to attack and inflict traumas.\n" +
        "Use BRAWN to defend and avoid taking wounds.\n\n" +
        "Focus on attacking players who are alone.\n\n" +
        "Kill the players to win.";

    public static string curseName = "the Gibbering Madness";

	// Use this for initialization
	public void StartCurse ()
    {
        isCursed = true;
        if (!gameObject.GetComponent<Player>().enabled)
            return;

        var collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 5f;
        InvokeRepeating("DoDamage", 1, 2);
	}

    public void OnTriggerEnter(Collider other)
    {
        if (!isCursed)
            return;

        if (other.gameObject != gameObject && other.gameObject.GetComponent<GibberingMadness>() != null)
        {
            Debug.Log("Added " + other.gameObject.name);
            playersInReach.Add(other.gameObject.GetComponent<Player>());
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!isCursed)
            return;

        if (other.gameObject != gameObject && other.gameObject.GetComponent<GibberingMadness>() != null && playersInReach.Contains(other.gameObject.GetComponent<Player>()))
        {
            Debug.Log("Removed " + other.gameObject.name);
            playersInReach.Remove(other.gameObject.GetComponent<Player>());
        }
    }
	
	// Update is called once per frame
	void DoDamage ()
    {
        if (gameObject.GetComponent<Player>().IsDead || playersInReach.Count(p=> !p.IsDead) <= 0 || Player.allPlayers.Any(p => !p.ReadyForCreakening))
            return;

        float opposingWill = playersInReach.Sum(p => p.Willpower.CurrentValue) / 2;
        float myWill = gameObject.GetComponent<Player>().Willpower.CurrentValue;

        float myCheck;
        float theirCheck;

        foreach (var player in playersInReach)
        {
            myCheck = UnityEngine.Random.Range(0f, 5f) + myWill;
            theirCheck = UnityEngine.Random.Range(0f, 5f) + opposingWill;

            if (myCheck > theirCheck)
            {
                CmdDamagePlayer(player.gameObject.name, 1, "Traumas");
                Debug.Log(String.Format("{0} has taken damage. Traumas: {1}", player.gameObject.name, player.Traumas.CurrentValue.ToString()));
            }
        }

        float opposingBrawn = playersInReach.Sum(p => p.Brawn.CurrentValue) / 2;
        float myBrawn = gameObject.GetComponent<Player>().Brawn.CurrentValue;

        myCheck = UnityEngine.Random.Range(0f, 5f) + myBrawn;
        theirCheck = UnityEngine.Random.Range(0f, 5f) + opposingBrawn;

        if (theirCheck > myCheck)
        {
            CmdDamagePlayer(gameObject.name, 1, "Wounds");
            Debug.Log(String.Format("{0} has taken damage. Wounds: {1}", gameObject.GetComponent<Player>().gameObject.name, gameObject.GetComponent<Player>().Wounds.CurrentValue.ToString()));
        }
	}

    [Command]
    void CmdDamagePlayer(string player, float damage, string stat)
    {
        Player hurtPLayer = GameObject.Find(player).GetComponent<Player>();
        hurtPLayer.RpcTakeDamage(damage, stat);
    }
}
