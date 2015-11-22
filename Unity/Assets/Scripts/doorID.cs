using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class doorID : NetworkBehaviour {

    [SyncVar]
    public string doorIdenitity;
    private Transform myTransform;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //SetIdentity();
	}

    public void SetIdentity()
    {
        doorIdenitity = "Door " + GetComponent<NetworkIdentity>().netId.ToString();

        //myTransform.parent.name = doorIdenitity;
        gameObject.name = doorIdenitity;
    }
}
