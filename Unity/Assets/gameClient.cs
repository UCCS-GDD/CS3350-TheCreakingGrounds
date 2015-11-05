using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class gameClient : NetworkBehaviour {

    [SyncVar]
    public bool startAwakening = false;

    [SyncVar]
    public bool awakeningCalled = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(isServer)
        {
            //Debug.Log("Server Update");
            if(startAwakening && awakeningCalled)
            {
                RpcStartAwaken();
            }
        }

        if(isClient)
        {
            //Debug.Log("Client Update");
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("PRESSED");
                CmdAwaken();
            }
            */
            if (!awakeningCalled)
            {
                StartCoroutine(changeBool());
            }
        }
	}

    IEnumerator changeBool()
    {
        /* SERVER
        Debug.Log("Called ChangedBool");
        awakeningCalled = true;
        yield return new WaitForSeconds(10f);
        Debug.Log("StartAwakinged TRUE");
        startAwakening = true;
        */ 

        //Client
        awakeningCalled = true;
        yield return new WaitForSeconds(10f);
        CmdAwaken();
    }

    //Called by player when player finds object
    [Command]
    public void CmdAwaken()
    {
        Debug.Log("Client to Server - CmdAwaken");
        startAwakening = true;
        Debug.Log("Is Server = " +isServer);
    }

    //Notify clients to start the Awakening Function
    [ClientRpc]
    public void RpcStartAwaken()
    {
        Debug.Log("Server to Client - RpcStartAwaken");
    }
}
