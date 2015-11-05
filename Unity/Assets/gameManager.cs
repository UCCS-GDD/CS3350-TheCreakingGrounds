using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class gameManager : NetworkBehaviour {

    [SyncVar]
    public bool awakeningStart = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Server]
    public void sendAwakening()
    {

    }

    [Client]
    void ShowExplosion()
    {
        
    }
}
