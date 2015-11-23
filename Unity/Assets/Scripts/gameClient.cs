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

    public awakenManager manager;

    [SyncVar]
    public string playerUniqueIdentity;
    public string uniqueName;

    [SyncVar]
    public bool startedAwakening;

    public override void OnStartLocalPlayer()
    {
        //Have server setup my name.
        CmdSetPlayerName();

        setupNames();
 
        base.OnStartLocalPlayer();
    }

	void Start () {
        awakeCanvas = Instantiate(awakeCanvasPrefab);
        awakeCanvas.gameObject.SetActive(false);
	}

    [Command]
    public void CmdSetPlayerName()
    {
        playerUniqueIdentity = "Player " + GetComponent<NetworkIdentity>().netId.ToString();
    }

    public void setupNames()
    {
        if (!isLocalPlayer)
        {
            gameObject.name = playerUniqueIdentity;
        }

        else
        {
            gameObject.name = playerUniqueIdentity;
        }
    }
	
	void Update () {
        if (gameObject.name == "" || gameObject.name == "Player - Network(Clone)")
        {
            setupNames();
        }
	}

    //CALLED BY PLAYER TO START AWAKENING. DO THIS!
    [Client]
    public void activateAwaken()
    {
        //string uIdentity = gameObject.transform.name;
        string whoStarted = gameObject.name;
        CmdAwaken(whoStarted);

        //Set the player's tag to Cursed
        gameObject.tag = "Cursed";
    }

    //Called by player to tell server to do this awaken
    [Command]
    public void CmdAwaken(string whoStarted)
    {
        Debug.Log("Client to Server - START AWAKEN - Is Server = " + isServer);

        //Change that client's startedAwakening to true
        startedAwakening = true;

        //Tell every player to find Server's Awaken Manager
        RpcClientFindManager();

        //Set name for who started it
        manager = GameObject.Find("ServerManagement").GetComponent<awakenManager>();
        manager.cursedPlayerName = whoStarted;

        //Server will choose a curse and proceed
        serverAwaken();
    }

    [ClientRpc]
    public void RpcClientFindManager()
    {
        //Find Awakening Manager
        manager = GameObject.Find("ServerManagement").GetComponent<awakenManager>();
    }

    [Server]
    private void serverAwaken()
    {
        //Randomly grab Curse name from list
        string curseName = awakeningList[Random.Range(0, awakeningList.Count - 1)];
        Debug.Log("Awaken = " + curseName);

        //Set the Awaken Name
        manager.curseName = curseName;

        //Find correct Awakening based off name
        //NOT USED TILL MORE ARE MADE

        //Inform all players that the awakening has happened
        RpcStartAwaken();
    }

    //Notify clients to start the Awakening Function
    [ClientRpc]
    public void RpcStartAwaken()
    {
        Debug.Log("Server to Client - START AWAKEN");

        //Setup Awakening Alert
        awakeCanvasText = awakeCanvas.transform.FindChild("CurseName").gameObject.GetComponent<Text>();
        awakeCanvasText.text = manager.curseName;
        awakeCanvas.gameObject.SetActive(true);

        //Start Awakening after delay
        StartCoroutine(startDelay(4f));
    }

    [Client]
    IEnumerator startDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //Start script of Game Mode Awakening
        GetComponent<GibberingMadness>().startGame();
    }
}