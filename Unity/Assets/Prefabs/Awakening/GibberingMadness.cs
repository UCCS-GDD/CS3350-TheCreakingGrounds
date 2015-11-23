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

    [SyncVar]
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

    List<GameObject> NotCursedPlayers;
    public GameObject cursedPlayer;
    public NetworkInstanceId curseID;

    public awakenManager manager;

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
        //If the game is setup and checks if all players are dead
        if(isSetup && !gameOver)
        {
            //If you're the cursed
            if (GetComponent<gameClient>().startedAwakening == true)
            {
                //IF PLAYERS ARE DEAD
                if (NotCursedPlayers.Count == 0)
                {
                    //END GAME. Only the Curse is left.
                    Debug.Log("All Not Cursed Players are dead. End Game.");

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

                //Ignore object if it is not a player or itself
                if(otherPlayer == null || otherPlayer == this)
                {
                    return;
                }

                //If the player is not safe, do damage
                if (otherPlayer.isSafe == false)
                {
                    Debug.Log("Cursed Damages Player");

                    //Do damage to other player
                    CmdTellServerWhoWasShot(otherPlayer.name, 1f);

                    //Sets delay for damage
                    canDamage = false;
                    StartCoroutine(damageDelay(3f));
                }
            }

            //Handling for Normal Player
            else
            {
                GibberingMadness otherPlayer = collider.gameObject.GetComponent<GibberingMadness>(); //Set other player's gibbering madness

                //Ignore object if it is not a player or itself
                if (otherPlayer == null || otherPlayer == this)
                {
                    return;
                }

                //If the the collided player is not the cursed
                if (otherPlayer.gameObject.GetComponent<gameClient>().startedAwakening != true)
                {
                    //isSafe = true;
                    CmdTellServerMySafeStatus(true);
                }

                //If the the collided player is the cursed & safe. Do damage.
                else if ( (otherPlayer.gameObject.GetComponent<gameClient>().startedAwakening != true) && isSafe)
                {
                    Debug.Log("Player Damages Cursed");

                    CmdTellServerWhoWasShot(otherPlayer.name, 1f);
                    //otherPlayer.TakeDamage(1f); //Deliver 5 damage

                    //Sets delay for damage
                    canDamage = false;
                    StartCoroutine(damageDelay(3f));
                }

                //Else he is not safe
                else
                {
                    //isSafe = false;
                    CmdTellServerMySafeStatus(false);
                }
            }
        }
    }

    //Called at the beggining of script initialization by server
    [Client]
    public void startGame()
    {
        Debug.Log("Gibbering Madness Started");

        //Find Awaken Manager
        manager = GameObject.Find("ServerManagement").GetComponent<awakenManager>();

        //Populate Uncursed Player list
        NotCursedPlayers = GameObject.FindGameObjectsWithTag("NotCursed").ToList();

        //Find Cursed Player
        cursedPlayer = GameObject.Find(manager.cursedPlayerName);
        Debug.Log("Cursed Player = " + cursedPlayer.name);

        //If the Player started the awakening
        if (gameObject.name.CompareTo(manager.cursedPlayerName) == 0)
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
            var instance = Instantiate(gibberingMadnessVia);
            instance.transform.SetParent(cursedPlayer.transform, false);
            instance = Instantiate(gibberingMadnessSound);
            instance.transform.SetParent(cursedPlayer.transform, false);
        }

        //Remove Alert Screen then start game
        StartCoroutine(DelayAndStart(3f));
    }

    IEnumerator DelayAndStart(float waitTime)
    {
        //Wait X seconds
        yield return new WaitForSeconds(waitTime);

        //Hide alert
        GetComponent<gameClient>().awakeCanvas.gameObject.SetActive(false);

        //Setup is complete
        isSetup = true; //SOMETHING WRONG HAPPENS HERE
    }

    IEnumerator damageDelay(float waitTime)
    {
        //Wait X seconds
        yield return new WaitForSeconds(waitTime);

        //Turn Damage back on
        canDamage = true;
    }

    [Command]
    void CmdTellServerMySafeStatus(bool status)
    {
        isSafe = status;
    }

    [Command]
    void CmdTellServerWhoWasShot(string player, float damage)
    {
        GameObject go = NotCursedPlayers.First(p => p.name.CompareTo(player) == 0).gameObject;
        go.GetComponent<GibberingMadness>().deductHealth(damage);
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
                //NotCursedPlayers.Remove(gameObject.GetComponent<Player>());
                NetworkServer.Destroy(gameObject);
                //MIGHT HAVE TO KEEP CAMERA ALIVE
                Instantiate(diedCanvas);
            }
        }
    }

    [Command]
    void CmdGameOver()
    {
        Debug.Log("Server Received = GAME OVER");
        gameOver = true;
    }

    [ClientRpc]
    void RpcGameOver()
    {
        Debug.Log("Client Received = GAME OVER");
        gameOver = true;
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

        //if you were the cursed and LOST
        if (GetComponent<gameClient>().startedAwakening == true)
        {
            Instantiate(loseCanvas);
        }

        //Else you were a player and WON
        else
        {
            Instantiate(winCanvas);
        }
    }

    //Cursed killed all the players
    [ClientRpc]
    public void RpcPlayersKilled()
    {
        Debug.Log("RpcPlayersKilled");

        //If you were the cursed and WON
        if (GetComponent<gameClient>().startedAwakening == true)
        {
            Instantiate(winCanvas);
        }

        //Else you were a player and LOST
        else
        {
            Instantiate(loseCanvas);
        }
    }
}
