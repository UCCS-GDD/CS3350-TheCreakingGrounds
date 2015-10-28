using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lobby : MonoBehaviour{

    public NetworkLobbyManager networkManager;
    public Text matchName;
    public Text numPlayers;

	// Use this for initialization
	void Start ()
    {
        //Find objects
        networkManager = GetComponent<NetworkManager>() as NetworkLobbyManager;
        //networkManager = GetComponent<NetworkLobbyManager>();
        matchName = GameObject.Find("MatchName").GetComponent<Text>();
        numPlayers = GameObject.Find("NumPlayers").GetComponent<Text>();

        //Setup Lobby Room
        matchName.text = networkManager.matchName;
        numPlayers.text = networkManager.numPlayers.ToString();

        //Attempt to become a player
        networkManager.TryToAddPlayer();
	}
	
	// Update is called once per frame
    void Update()
    {
        //Display correct number of player count
        numPlayers.text = networkManager.numPlayers.ToString();
    }
}
