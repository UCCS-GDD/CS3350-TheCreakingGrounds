using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class interactManager : NetworkBehaviour {

    //Container ID Listing
    public SyncListUInt containerIDList = new SyncListUInt();

    //Container Listing
    public GameObject[] containerList;

    //Door Listing
    public GameObject[] doorList;

    //Wait times
    public float waitToStart;
    public float waitEachSetup;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(setupLevel(waitToStart));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator setupLevel(float waitTime)
    {
        Debug.Log("Waiting to setup level");
        yield return new WaitForSeconds(waitTime);

        StartCoroutine(doorSetup(waitEachSetup));
    }

    IEnumerator doorSetup(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Door setup Starting");

        //Wait and rename all doors
        doorList = GameObject.FindGameObjectsWithTag("Door");

        //Go through list and make changes
        foreach( GameObject f in doorList)
        {
            f.GetComponent<doorID>().SetIdentity();
        }

        //Setup Containers next
        StartCoroutine(containerSetup(waitEachSetup));
    }

    IEnumerator containerSetup(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Container Setup Starting");

        //Wait and rename all doors
        containerList = GameObject.FindGameObjectsWithTag("Container");

        //Go through list and make changes
        foreach (GameObject f in containerList)
        {
            //Setup names
            f.GetComponent<containerID>().SetIdentity();

            //If you're the server, setup the containerID list
            if(isServer)
            {
                uint x = f.GetComponent<NetworkIdentity>().netId.Value;
                containerIDList.Add(x);
            }
        }
    }
}
