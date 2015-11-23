using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class awakenManager : NetworkBehaviour {

    [SyncVar]
    public string cursedPlayerName;

    [SyncVar]
    public string curseName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
