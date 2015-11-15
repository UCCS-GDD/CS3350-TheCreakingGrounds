using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class GuiLobbyManager : NetworkLobbyManager
{
    public LobbyCanvasControl lobbyCanvas;
    public OfflineCanvasControl offlineCanvas;
    public OnlineCanvasControl onlineCanvas;
    public ConnectingCanvasControl connectingCanvas;
    public PopupCanvasControl popupCanvas;

    public GameObject charCreation;
    public Button charShowButton;

    public string onlineStatus;
    static public GuiLobbyManager s_Singleton;

    void Start()
    {
        s_Singleton = this;
        offlineCanvas.Show();

        //charCreation = GameObject.Find("CharacterCreation");
        //charCreation.SetActive(false);
    }

    //Determine which canvas
    void OnLevelWasLoaded()
    {
        if (lobbyCanvas != null) lobbyCanvas.OnLevelWasLoaded();
        if (offlineCanvas != null) offlineCanvas.OnLevelWasLoaded();
        if (onlineCanvas != null) onlineCanvas.OnLevelWasLoaded();
        if (connectingCanvas != null) connectingCanvas.OnLevelWasLoaded();
        if (popupCanvas != null) popupCanvas.OnLevelWasLoaded();
    }

    public void SetFocusToAddPlayerButton()
    {
        if (lobbyCanvas == null)
            return;

        lobbyCanvas.SetFocusToAddPlayerButton();
    }

    /*
     * Server Callbacks
    */ 

    public override void OnLobbyStopHost()
    {
        lobbyCanvas.Hide();
        offlineCanvas.Show();
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //This hook allows you to apply state data from the lobby-player to the game-player
        return true;
    }

    /*
     * Client Callbacks
    */ 

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        connectingCanvas.Hide();
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        connectingCanvas.Hide();
        StopHost();

        popupCanvas.Show("Client Error", errorCode.ToString());
    }

    public override void OnLobbyClientDisconnect(NetworkConnection conn)
    {
        lobbyCanvas.Hide();
        offlineCanvas.Show();
    }

    public override void OnLobbyStartClient(NetworkClient client)
    {
        if (matchInfo != null)
        {
            connectingCanvas.Show(matchInfo.address);
        }
        else
        {
            connectingCanvas.Show(networkAddress);
        }
    }

    public override void OnLobbyClientAddPlayerFailed()
    {
        popupCanvas.Show("Error", "No more players allowed.");
    }

    public override void OnLobbyClientEnter()
    {
        lobbyCanvas.Show();
        onlineCanvas.Show(onlineStatus);

        //Find buttons for Char creation
        charShowButton = GameObject.Find("LobbyCanvas(Clone)/Panel/BuildCharacterButton").GetComponent<Button>();

        //Activate button
        charShowButton.onClick.AddListener(() => showCharCreator());
    }

    public override void OnLobbyClientExit()
    {
        lobbyCanvas.Hide();
        onlineCanvas.Hide();
    }

    public void showCharCreator()
    {
        charCreation.SetActive(true);
        GameObject.Find("CharacterCreation/Player").GetComponent<AudioListener>().enabled = false;
    }

    public void hideCharCreator()
    {
        charCreation.SetActive(false);
    }
}
