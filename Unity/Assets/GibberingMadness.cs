using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GibberingMadness : MonoBehaviour {

    public bool isSetup;

	// Use this for initialization
	void Start () {
        isSetup = false;

        //awakeCanvas = GetComponent<gameClient>().awakeCanvas;
        //GetComponent<gameClient>().awakeCanvasText.text = ""
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If the game is setup and ready to start checking for things
        if(isSetup)
        {

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
}
