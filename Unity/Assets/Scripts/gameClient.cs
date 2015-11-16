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

    [SyncVar]
    public string playerUniqueIdentity;
    private NetworkInstanceId playerNetID;
    private Transform myTransform;
    public string uniqueName;

    [SyncVar]
    public bool startedAwakening;

    private bool testStarted = false;

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
        SetIdenity();
        
        //base.OnStartLocalPlayer();
    }

	// Use this for initialization
	void Start () {
        awakeCanvas = Instantiate(awakeCanvasPrefab);
        awakeCanvas.gameObject.SetActive(false);
	}

    void Awake()
    {
        myTransform = transform;
    }

    [Client]
    private void GetNetIdentity()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }
    private string MakeUniqueIdentity()
    {
        string uniqueName = "Player " + playerNetID.ToString();
        return uniqueName;
    }

    [Command]
    private void CmdTellServerMyIdentity(string name)
    {
        playerUniqueIdentity = name;
    }

    void SetIdenity()
    {
        if(!isLocalPlayer)
        {
            myTransform.name = playerUniqueIdentity;
        }

        else
        {
            myTransform.name = MakeUniqueIdentity();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (myTransform.name == "" || myTransform.name == "Player - Network(Clone)")
        {
            SetIdenity();
        }

        return;
        if(isServer)
        {

        }

        if(isClient && isServer)
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
        //string uIdentity = gameObject.transform.name;
        string whoStarted = gameObject.name;
        CmdAwaken(whoStarted);
    }

    //Called by player to tell server to do this awaken
    [Command]
    public void CmdAwaken(string whoStarted)
    {
        Debug.Log("Client to Server - START AWAKEN - Is Server = " + isServer);
        startedAwakening = true; //Change that client's startedAwakening to true

        serverAwaken(whoStarted);
    }

    [Server]
    private void serverAwaken( string whoStarted )
    {
        //Randomly grab Curse name from list
        string awakeName = awakeningList[Random.Range(0, awakeningList.Count - 1)];
        Debug.Log("Awaken = " + awakeName);

        //Find correct Awakening based off name
        //NOT USED TILL MORE ARE MADE

        RpcStartAwaken(awakeName, whoStarted);
    }

    //Notify clients to start the Awakening Function
    [ClientRpc]
    public void RpcStartAwaken(string awakeName, string whoStarted)
    {
        Debug.Log("Server to Client - START AWAKEN");
        //clientAwaken(name, uIdentity);

        //Setup Awakening Alert
        awakeCanvasText = awakeCanvas.transform.FindChild("CurseName").gameObject.GetComponent<Text>();
        awakeCanvasText.text = awakeName;
        awakeCanvas.gameObject.SetActive(true);

        //Start Awakening after delay
        StartCoroutine(startDelay(3f, whoStarted));
    }

    [Client]
    IEnumerator startDelay(float waitTime, string whoStarted)
    {
        yield return new WaitForSeconds(waitTime);

        //Start script of Game Mode Awakening
        GetComponent<GibberingMadness>().startGame(whoStarted);
    }
}