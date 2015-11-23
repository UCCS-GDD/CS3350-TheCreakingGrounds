using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class gameClient : NetworkBehaviour {
    [SyncVar]
    public string playerUniqueIdentity;
    public string uniqueName;

    public GameObject myObject;

    public override void OnStartLocalPlayer()
    {
        //Have server setup my name.
        CmdSetPlayerName();

        setupNames();
 
        base.OnStartLocalPlayer();
    }

	void Start () {
        myObject = gameObject;
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
}