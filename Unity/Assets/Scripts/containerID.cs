using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class containerID : NetworkBehaviour{

    [SyncVar]
    public string containerIdenitity;
    private Transform myTransform;

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update()
    {
        //SetIdentity();
    }

    public void SetIdentity()
    {
        containerIdenitity = "Container " + GetComponent<NetworkIdentity>().netId.ToString();

        gameObject.name = containerIdenitity;
    }
}
