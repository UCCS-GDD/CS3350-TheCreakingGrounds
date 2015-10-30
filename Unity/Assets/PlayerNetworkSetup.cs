using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

    [SerializeField]
    public Camera PlayerCamera;
    [SerializeField]
    public AudioListener audioListener;

    public GameObject spawnLocation;
    //public GameObject playerUI;
     

    //public Camera PlayerCamera;
    //public AudioListener audioListener;

	// Use this for initialization
	void Start ()
    {
        //If this script started is the local player
        if(isLocalPlayer)
        {
            //Disable the MainCamera which may be by default in the scene
            //GameObject.Find("SceneCamera").SetActive(false);

            //Activate the Player Script on the player
            GetComponent<Assets.Scripts.Player>().enabled = true;

            //Turn on First Person Camera
            PlayerCamera.gameObject.SetActive(true);

            //Activate Audio Listener
            audioListener.enabled = true;

            //Fix Reticle Issue
            //Instantiate(playerUI, player.transform.position, Quaternion.identity);

            //Set player to SpawnLocation
            spawnLocation = GameObject.Find("SpawnLocation");
            gameObject.transform.position = spawnLocation.transform.position + new Vector3(Random.Range(-5F, 5F), 0, Random.Range(-5F, 5F));
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
