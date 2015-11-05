using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class gameClient : NetworkBehaviour {

    public List<string> awakeningList;
    public Canvas awakeCanvasPrefab;
    public Canvas awakeCanvas;
    public Text awakeCanvasText;

    public bool startedAwakening;

    private bool testStarted = false;

	// Use this for initialization
	void Start () {
        awakeCanvas = Instantiate(awakeCanvasPrefab);
        awakeCanvas.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if(isServer)
        {

        }

        if(isClient && !isServer)
        {
            //Test Awaken
            if(!testStarted)
            {
                testStarted = true;
                StartCoroutine(awakeTest());
            }
        }

        if(isClient)
        {

        }
	}

    //Used only for testing the awake
    IEnumerator awakeTest()
    {
        yield return new WaitForSeconds(10f);
        activateAwaken();
    }

    //CALLED BY PLAYER TO START AWAKENING. DO THIS!
    [Client]
    public void activateAwaken()
    {
        startedAwakening = true;
        CmdAwaken();
    }

    //Called by player to tell server to do this awaken
    [Command]
    public void CmdAwaken()
    {
        Debug.Log("Client to Server - START AWAKEN - Is Server = " + isServer);
        serverAwaken();
    }

    //Server setup for awakening
    [Server]
    public void serverAwaken()
    {
        //Randomly grab Curse name from list
        string awakeName = awakeningList[Random.Range(0, awakeningList.Count - 1)];
        Debug.Log("Awaken = " + awakeName);

        //Find correct Awakening based off name
        //NOT USED TILL MORE ARE MADE

        RpcStartAwaken(awakeName);
    }

    //Notify clients to start the Awakening Function
    [ClientRpc]
    public void RpcStartAwaken(string name)
    {
        Debug.Log("Server to Client - START AWAKEN");
        clientAwaken(name);
    }

    //Client main function to start the awakening. Called by clients.
    [Client]
    public void clientAwaken(string name)
    {
        //Setup Awakening Alert
        awakeCanvasText = awakeCanvas.transform.FindChild("CurseName").gameObject.GetComponent<Text>();
        awakeCanvasText.text = name;
        awakeCanvas.gameObject.SetActive(true);

        //Start Awakening after delay
        StartCoroutine(startDelay(3f));
    }

    [Client]
    IEnumerator startDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //Start script of Game Mode Awakening
        GetComponent<GibberingMadness>().startGame();
    }
}