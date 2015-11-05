using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using Assets.Scripts;

public class GibberingMadness : NetworkBehaviour {

    public bool isSetup;
    public GameObject enemy;

    public Canvas winCanvas;
    public Canvas loseCanvas;
    public Canvas exitCurrentGame;

	// Use this for initialization
	void Start () {
        isSetup = false;
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
                //IF PLAYERS ARE DEAD
                //END GAME

                //ELSE, PLAY GAME AND DO THIS BELOW
                //Check position distance of players AND check if they're notSafe. IF DAMAGE, ELSE DON'T.
                //HERE

                //GibberingMadness otherPlayer = collision.gameObject.GetComponent<GibberingMadness>(); //Set other player's gibbering madness
                //otherPlayer.TakeDamage(5f); //Deliver 5 damage
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
            //HERE

            //Remove Alert Screen then start game
            StartCoroutine(DelayAndStart(3f));
        }

        //If they didn't start the awakening
        else
        {
            //Change text
            GetComponent<gameClient>().awakeCanvasText.text = "Kill The Cursed";

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

    public void TakeDamage(float amount)
    {
        //Get this objects Player script
        Player thisPlayer = GetComponent<Player>();

        //Reduce trauma
        //thisPlayer.Traumas.CurrentValue -= amount;

        //Checks if trauma is nothing
        if( thisPlayer.Traumas.CurrentValue > 1 )
        {
            //Kill player
            NetworkServer.Destroy(gameObject);
            return;
        }
    }
}
