using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Assets.Scripts;
using UnityEngine.UI;

public class awakenManager : NetworkBehaviour {

    [SyncVar]
    public string curseName;

    [SyncVar]
    public string cursedPlayerName;

    public GameObject handlePlayer; //Player this manager should worry about locally

    public List<string> awakeningList;

	// Use this for initialization
	void Start () {
        if(isServer)
        {
            //Randomly grab Curse name from list
            string choosen = awakeningList.PickRandom();
            Debug.Log("Awaken = " + choosen);

            //Set the Awaken Name
            curseName = choosen;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void setHandle(GameObject handleThis)
    {
        handlePlayer = handleThis;
    }

    public void startAsCurse()
    {
        var cursedPlayer = GameObject.Find(cursedPlayerName);

    }

    public void startAsNormal()
    {
        var cursedPlayer = GameObject.Find(cursedPlayerName);

    }
}
