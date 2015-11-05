using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using Assets.Scripts;

public class GibberingMadness : NetworkBehaviour {

    public bool isSetup;

    public GameObject enemy;
    public SphereCollider collider;
    public bool isSafe;
    private bool canDamage;

    [SyncVar]
    private int health;

    public Canvas winCanvas;
    public Canvas loseCanvas;
    //public Canvas exitCurrentGame;

	// Use this for initialization
	void Start () {
        isSetup = false;
        canDamage = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If the game is setup and ready to start checking for things
        if(isSetup)
        {
            //If you're the cursed
            if( GetComponent<gameClient>().startedAwakening == true )
            {
                //IF PLAYERS ARE DEAD. FIX
                if(false)
                {
                    //END GAME
                }
            }

            //If you're normal
            else
            {
                //IF ENEMY IS DEAD, DO THIS

                //Check distance from group of players. IF !notSafe, ELSE notSAFE
                //HERE
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

                if(otherPlayer == null)
                {
                    return;
                }

                //If the player is not safe, do damage
                if (otherPlayer.isSafe == false)
                {
                    
                    doDamage(1f, otherPlayer.gameObject);
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

                if (otherPlayer == null)
                {
                    return;
                }

                //If the the collided player is not the cursed
                if (otherPlayer.GetComponent<gameClient>().startedAwakening == false)
                {
                    isSafe = true;
                }

                //If the the collided player is not the cursed
                if (otherPlayer.GetComponent<gameClient>().startedAwakening == true && isSafe)
                {
                    doDamage(1f, otherPlayer.gameObject);
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
    public void startGame()
    {
        Debug.Log("Gibbering Madness Started");

        //If the Player started the awakening
        if( GetComponent<gameClient>().startedAwakening == true )
        {
            //Change text
            GetComponent<gameClient>().awakeCanvasText.text = "Kill Your Friends";

            //Change players model to the gas
            //HERE

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

    public void doDamage(float amount, GameObject player)
    {
        string uIdentity = player.transform.name;
        CmdTellServerWhoWasShot(uIdentity, amount);
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
                CmdEnemyKilled();
                return;
            }

            //Player killed
            else
            {
                Debug.Log("Played Killed");
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    [Command]
    void CmdTellServerWhoWasShot(string uniqueID, float damage)
    {
        GameObject go = GameObject.Find(uniqueID);
        go.GetComponent<GibberingMadness>().deductHealth(damage);
    }

    //Called when enemy is killed. Informs other players that it is killed and brings up win canvas for them.
    [Command]
    public void CmdEnemyKilled()
    {
        RpcDetermineWinEnemyKilled();
    }

    //Called when players are killed.
    [Command]
    public void CmdPlayersKilled()
    {
        RpcDetermineWinPlayersKilled();
    }

    //Called by server to client. Determines if the player won or lost.
    [ClientRpc]
    public void RpcDetermineWinEnemyKilled()
    {
        //If you were the enemy killed, show lose
        if( GetComponent<gameClient>().startedAwakening == true )
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
    public void RpcDetermineWinPlayersKilled()
    {
        //If you were the enemy killed, show lose
        if (GetComponent<gameClient>().startedAwakening == true)
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
