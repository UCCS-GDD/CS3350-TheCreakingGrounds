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
        if (playersInReach.Count <= 0)
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
