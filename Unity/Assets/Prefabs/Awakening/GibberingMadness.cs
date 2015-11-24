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

    public static string playerBriefing = "You are a Survivor. Work together with the others to defeat the Betrayer. " +
        "In this Curse, the Betrayer is the Gibbering Madness, an incarnation of a murderous, insane spirit. " +
        "It will seek out players who are alone, so try to find your friends – in a group, you may be able to defeat them. " +
        "You will want to increase your willpower, to avoid the Gibbering Madness from killing you easily, so try continuing to search the mansion. " +
        "The Gibbering Madness will kill you just by being near you if you are on your own. Get too many Traumas, and you will die. " +
        "If the Betrayer fails to inflict Traumas then they will receive wounds until they die.\n\nKill the Betrayer to win.\nIf all Survivors die, you lose.";

    public static string curseBriefing = "\tYou are the Gibbering Madness, cursed and possessed by the insane spirits that dwell in the mansion. " +
        "Your mission is to kill the party. You do this simply by being near players. " +
        "The longer you are near them, the more traumas they take until they perish. " +
        "However, be cautious. If you are affecting multiple players at once, they will gain greater resistances and make it quite difficult for you to deal damage. " +
        "If you fail to deal damage, you will take a Wound. Get all your wounds and you will die.\n\nKill all the players to win.\nDie and you lose.";

    public static string curseName = "the Gibbering Madness";

	// Use this for initialization
	public void StartCurse () 
    {
        if (!gameObject.GetComponent<Player>().enabled)
            return;

        isCursed = true;
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
        if (playersInReach.Count(p=> !p.IsDead) <= 0 || Player.allPlayers.Any(p => !p.ReadyForCreakening))
            return;

        float opposingScore = playersInReach.Sum(p => p.Willpower.CurrentValue) / 2;
        float myScore = gameObject.GetComponent<Player>().Willpower.CurrentValue;

        float myCheck;
        float theirCheck;

        foreach (var player in playersInReach)
        {
            myCheck = UnityEngine.Random.Range(0f, 5f) + myScore;
            theirCheck = UnityEngine.Random.Range(0f, 5f) + opposingScore;

            if (myCheck > theirCheck)
            {
                CmdDamagePlayer(player.gameObject.name, 1, "Traumas");
                Debug.Log(String.Format("{0} has taken damage. Traumas: {1}", player.gameObject.name, player.Traumas.CurrentValue.ToString()));
            }
        }

        myCheck = UnityEngine.Random.Range(0f, 5f) + myScore;
        theirCheck = UnityEngine.Random.Range(0f, 5f) + opposingScore;

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
