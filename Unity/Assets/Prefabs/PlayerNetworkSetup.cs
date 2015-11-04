using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Assets.Scripts;

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
            //Check if the scene currently is the Mansion
            if (Application.loadedLevelName.Contains("Mansion"))
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
                
                //Setup player stats
                setupPlayerStats();

                //Set player to SpawnLocation
                spawnLocation = GameObject.Find("SpawnLocation");
                gameObject.transform.position = spawnLocation.transform.position + new Vector3(Random.Range(-1F, 1F), 0, Random.Range(-1F, 1F));

                //Disable Spawnlocation
                spawnLocation.SetActive(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Setup the player stats and perks upon start
    public void setupPlayerStats()
    {
        //Check if file exists. If so, read it and set stats
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        { //If save file exists
            //READ FILE AND SET INTEGERS CORRECTLY
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            //Deserialize game so it can be understood
            PlayerData data = (PlayerData)bf.Deserialize(file);

            //Close file since we have loaded the file into game
            file.Close();

            //Set variables from load
            Player player = gameObject.GetComponent<Player>();
            player.Brawn = new Stat(data.brawn);
            player.Speed = new Stat(data.speed);
            player.Intellect = new Stat(data.intellect);
            player.Willpower = new Stat(data.willpower);
            Perk perk = Resources.LoadAll<Perk>("Data/Perks").FirstOrDefault(p => p.Name == data.perk);
            if (perk != null)
                player.Perks.Add(perk);
            Transform models = gameObject.transform.FindChild("Model");
            for (int i = 0; i < models.childCount; i++ )
            {
                var child = models.GetChild(i);
                child.SetParent(null);
                Destroy(child.gameObject);
            }
            GameObject model = Resources.Load<GameObject>("CharacterModels/" + data.model);
            model.transform.SetParent(models, false);

            Debug.Log("Character Loaded: " + Application.persistentDataPath + "/playerInfo.dat");
        }
        else
        {
            Debug.Log("ERROR LOADING SAVE FILE FROM LOAD FILE()");
        }
    }
}
