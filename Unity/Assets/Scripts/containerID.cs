using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class containerID : NetworkBehaviour{

    [SyncVar]
    public string containerIdenitity;
    private Transform myTransform;

    // Use this for initialization
    void Start () {
        myTransform = transform;
        containerIdenitity = "Container " + GetComponent<NetworkIdentity>().netId.ToString();

        SetIdentity();
    }

    // Update is called once per frame
    void Update()
    {
        //SetIdentity();
    }

    private void SetIdentity()
    {
        //myTransform.parent.name = doorIdenitity;
        myTransform.name = containerIdenitity;
    }
}
