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
                    Debug.Log("Doing Damage");
                    otherPlayer.TakeDamage(1f); //Deliver 5 damage

                    //Sets delay for damage
                    canDamage = false;
                    StartCoroutine(damageDelay(3f));
                }

                //Else, receive damage for being too close
                else
                {
                    Debug.Log("Taking Damage");
                    TakeDamage(5f);
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

    public void TakeDamage(float amount)
    {
        //Get this objects Player script
        Player thisPlayer = GetComponent<Player>();

        //Reduce trauma
        thisPlayer.Traumas.DamageStat(amount);

        Debug.Log("Health = " + thisPlayer.Traumas.CurrentValue);

        //Checks if trauma is nothing
        if( thisPlayer.Traumas.CurrentValue < 1 )
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
