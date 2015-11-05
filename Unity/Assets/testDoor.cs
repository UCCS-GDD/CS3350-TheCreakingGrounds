using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Assets.Scripts;

public class testDoor : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if( GetComponent<Openable>().isOpen == true )
        {
            changeBool(true);
        }
	}

    [Server]
    void changeBool (bool changeTo)
    {

    }
}
