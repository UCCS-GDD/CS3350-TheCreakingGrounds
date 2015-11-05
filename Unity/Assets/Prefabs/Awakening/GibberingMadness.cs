using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using Assets.Scripts;

public class GibberingMadness : NetworkBehaviour {

    public bool isSetup;

    [SyncVar]
    public bool gameOver;

    public bool isMe;

    public GameObject enemy;
    public SphereCollider collider;
    public bool isSafe;
    private bool canDamage;

    [SyncVar]
    private int health;

    public Canvas winCanvas;
    public Canvas loseCanvas;
    public Canvas diedCanvas;
    //public Canvas exitCurrentGame;

    public GameObject gibberingMadnessVia;
    public GameObject gibberingMadnessSound;

    public GameObject curseObject;
    public NetworkInstanceId curseID;

    List<Player> players;

    public List<Player> Players
    {
        get 
        {
            if (players == null)
                players = Object.FindObjectsOfType<Player>().ToList();
            return players;
        }
    }

	// Use this for initialization
	void Start () {
        isMe = true;
        isSetup = false;
        canDamage = true;
        gameOver = false;

        //Change names of other player objects
	}
	
	// Update is called once per frame
	void Update ()
    {

        //If the game is setup and ready to start checking for things
        if(isSetup && !gameOver)
        {
            //If you're the cursed
            if (GetComponent<gameClient>().startedAwakening == true)
            {
                //IF PLAYERS ARE DEAD
                if(Players.Count == 1)
                {
                    //END GAME. Only the Curse is left.
                    Debug.Log("Player Count = 1. End Game, Dead Players.");

                    CmdGameOver();

                    CmdPlayersKilled();
                }
            }
        }
	}

    void OnTriggerStay( Collider collider )
    {
        if (isSetup && canDamage)
        {
            //Handling for Cursed Player
            if (GetComponent<gameClient>().startedAwakening == true)
            {
                GibberingMadness otherPlayer = collider.gameObject.GetComponent<GibberingMadness>(); //Set other player's gibbering madness

                if(otherPlayer == null || otherPlayer == this)
                {
                    return;
                }

                //If the player is not safe, do damage
                if (otherPlayer.isSafe == false)
                {
                    Debug.Log("Cursed Damages Player");

                    doDamage(1f, otherPlayer.GetComponent<NetworkIdentity>().netId);
                    //otherPlayer.TakeDamage(1f); //Deliver 5 damage

                    //Sets delay for damage
                    canDamage = false;
                    StartCoroutine(damageDelay(3f));
                }
            }

            //Handling for Normal Player
            else
            {
                GibberingMadness otherPlayer = collider.gameObject.GetComponent<GibberingMadness>(); //Set other player's gibbering madness

                if (otherPlayer == null || otherPlayer == this)
                {
                    return;
                }

                //If the the collided player is not the cursed
                if (otherPlayer.gameObject != curseObject)
                {
                    isSafe = true;
                }

                //If the the collided player is the cursed & safe. Do damage.
                else if (otherPlayer.gameObject == curseObject && isSafe)
                {
                    Debug.Log("Player Damages Cursed");

                    doDamage(1f, curseID);
                    //otherPlayer.TakeDamage(1f); //Deliver 5 damage

                    //Sets delay for damage
                    canDamage = false;
                    StartCoroutine(damageDelay(3f));
                }

                //Else he is not safe
                else
                {
                    isSafe = false;
                }
            }
        }
    }

    //Called at the beggining of script initialization by server
    public void startGame(NetworkInstanceId uIdentity)
    {
        Debug.Log("Gibbering Madness Started");

        curseID = uIdentity;
        curseObject = Players.First(p => p.GetComponent<NetworkIdentity>().netId == uIdentity).gameObject;
        Debug.Log(curseObject.name);

        //myLobby = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager>();
        //findCursed();

        //If the Player started the awakening
        if (GetComponent<gameClient>().startedAwakening == true)
        {
            //Change text
            GetComponent<gameClient>().awakeCanvasText.text = "Kill Your Friends";

            //Change players model to the gas
            var instance = Instantiate(gibberingMadnessVia);
            instance.transform.SetParent(gameObject.transform, false);

            //Add a Circle Trigger damage radius around player
            collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = 5;

            //Remove Alert Screen then start game
            StartCoroutine(DelayAndStart(3f));
        }

        //If they didn't start the awakening
        else
        {
            //Change text
            GetComponent<gameClient>().awakeCanvasText.text = "Kill The Cursed";

            //Add player circle
            collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = 3;

            //find the betrayer and spawn shit
            //Player awakened = Players.First(p => p.GetComponent<gameClient>().startedAwakening);
            var instance = Instantiate(gibberingMadnessVia);
            instance.transform.SetParent(curseObject.transform, false);
            instance = Instantiate(gibberingMadnessSound);
            instance.transform.SetParent(curseObject.transform, false);

            //Remove Alert Screen then start game
            StartCoroutine(DelayAndStart(3f));
        }
    }

    IEnumerator DelayAndStart(float waitTime)
    {
        //Wait X seconds
        yield return new WaitForSeconds(waitTime);

        //Hide alert
        GetComponent<gameClient>().awakeCanvas.gameObject.SetActive(false);

        //Setup is complete
        isSetup = true;
    }

    IEnumerator damageDelay(float waitTime)
    {
        //Wait X seconds
        yield return new WaitForSeconds(waitTime);

        //Turn Damage back on
        canDamage = true;
    }

    public void doDamage(float amount, NetworkInstanceId player)
    {
        //string uIdentity = player.transform.name;
        CmdTellServerWhoWasShot(player, amount);
    }

    public void deductHealth (float dmg)
    {
        Debug.Log("Getting Damaged");

        //Reduce trauma
        GetComponent<Player>().Traumas.DamageStat(dmg);

        Debug.Log("Health = " + GetComponent<Player>().Traumas.CurrentValue);

        //Checks if trauma is nothing
        if (GetComponent<Player>().Traumas.CurrentValue < 1)
        {
            //If the enemy is killed, inform server
            if (GetComponent<gameClient>().startedAwakening == true)
            {
                Debug.Log("Cursed Killed");

                CmdGameOver();

                //End Game. Curse is killed.
                CmdCurseKilled();
                return;
            }

            //Player killed
            else
            {
                Debug.Log("Played Killed");
                Players.Remove(gameObject.GetComponent<Player>());
                Instantiate(diedCanvas);
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    [Command]
    void CmdGameOver()
    {
        gameOver = true;
    }

    [Command]
    void CmdTellServerWhoWasShot(NetworkInstanceId uniqueID, float damage)
    {
        GameObject go = Players.First(p => p.GetComponent<NetworkIdentity>().netId == uniqueID).gameObject;
        go.GetComponent<GibberingMadness>().deductHealth(damage);
    }

    //Called when enemy is killed. Informs other players that it is killed and brings up win canvas for them.
    [Command]
    public void CmdCurseKilled()
    {
        RpcCurseKilled();
    }

    //Called when players are killed.
    [Command]
    public void CmdPlayersKilled()
    {
        RpcPlayersKilled();
    }

    //Called by server to client. Determines if the player won or lost.
    [ClientRpc]
    public void RpcCurseKilled()
    {
        Debug.Log("RpcCurseKilled");

        //If you were the enemy killed, show lose
        if (gameObject == curseObject)
        {
            Instantiate(loseCanvas);
        }

        //Else you were a player that won
        else
        {
            Instantiate(winCanvas);
        }
    }

    //Called by server to client. Determines if the player won or lost.
    [ClientRpc]
    public void RpcPlayersKilled()
    {
        Debug.Log("RpcPlayersKilled");

        //If you were the enemy killed, show lose
        if (gameObject == curseObject)
        {
            Instantiate(winCanvas);
        }

        //Else you were a player that won
        else
        {
            Instantiate(loseCanvas);
        }
    }
}
