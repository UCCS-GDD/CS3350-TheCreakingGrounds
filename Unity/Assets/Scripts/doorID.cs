using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class doorID : NetworkBehaviour {

    [SyncVar]
    public string doorIdenitity;
    private Transform myTransform;

	// Use this for initialization
	void Start () {
        myTransform = transform;
        doorIdenitity = "Door " + GetComponent<NetworkIdentity>().netId.ToString();

        SetIdentity();
	}
	
	// Update is called once per frame
	void Update () {
        //SetIdentity();
	}

    private void SetIdentity()
    {
        //myTransform.parent.name = doorIdenitity;
        myTransform.name = doorIdenitity;
    }
}
