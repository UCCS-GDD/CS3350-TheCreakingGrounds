using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
//using System.Linq;
//using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    private GameObject MainMenuCanvas;
    private GameObject HostGameCanvas;
    private GameObject JoinGameCanvas;
    private GameObject OptionsGameCanvas;
    private GameObject QuitGameCanvas;
    private Button BackButton;

    public NetworkLobbyManager networkManager;
    public InputField hostName;
    public InputField hostPortNumber;
    public InputField maxPlayers;
    public InputField IPAddress;
    public InputField joinPortNumber;

    void Start()
    {
        //Find objects
        MainMenuCanvas = GameObject.Find("MainMenu");
        HostGameCanvas = GameObject.Find("HostGame");
        JoinGameCanvas = GameObject.Find("JoinGame");
        OptionsGameCanvas = GameObject.Find("OptionsGame");
        QuitGameCanvas = GameObject.Find("QuitGame");
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();
        //networkManager = GetComponent<NetworkManager>();

        //Setup objects
        BackButton.gameObject.SetActive(false);
        MainMenuShow();
    }

    void Update()
    {

    }

    /*
     *Main Menu button functions
     */

    public void HostGame()
    {
        //Show and hide objects
        Debug.Log("Host Game SELECTED");
        MainMenuCanvas.SetActive(false);
        HostGameCanvas.SetActive(true);
        BackButton.gameObject.SetActive(true);
    }

    public void JoinGame()
    {
        Debug.Log("Join Game SELECTED");
        MainMenuCanvas.SetActive(false);
        JoinGameCanvas.SetActive(true);
        BackButton.gameObject.SetActive(true);
    }

    public void Options()
    {
        Debug.Log("Options SELECTED");
        MainMenuCanvas.SetActive(false);
        OptionsGameCanvas.SetActive(true);
        BackButton.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Exit SELECTED");
        MainMenuCanvas.SetActive(false);
        QuitGameCanvas.SetActive(true);
        //BackButton.gameObject.SetActive(true);
    }

    //Used by back button primarily
    public void MainMenuShow()
    {
        HostGameCanvas.SetActive(false);
        JoinGameCanvas.SetActive(false);
        OptionsGameCanvas.SetActive(false);
        QuitGameCanvas.SetActive(false);
        BackButton.gameObject.SetActive(false);

        MainMenuCanvas.SetActive(true);
    }

    /*
     * Host Game Functions
     */
    public void startHost()
    {
        Debug.Log("Starting Host");

        //NOTHING DONE WITH HOST NAME YET

        //Port Setup
        if (hostPortNumber.text.CompareTo("") == 0)
        {
            Debug.Log("Empty Port, assigning 7777");
            networkManager.networkPort = 7777;
        }
        else
        {
            networkManager.networkPort = int.Parse(hostPortNumber.text);
        }

        //Max Players Setup
        if (maxPlayers.text.CompareTo("") == 0)
        {
            Debug.Log("Empty Max Players, assigning 6");
            networkManager.maxPlayers = 6;
        }
        else
        {
            networkManager.maxPlayers = int.Parse(maxPlayers.text);
        }
        
        //NOTHING DONE WITH PRIVATE HOST OPTION

        Application.LoadLevel(1);
        networkManager.StartHost();
    }

    /*
     * Join Game Functions
     */
    public void startJoin()
    {
        Debug.Log("Starting Join");

        //IP Address Setup
        if(IPAddress.text.CompareTo("") == 0)
        {
            Debug.Log("Empty IP Address, assign localhost");
            networkManager.networkAddress = "localhost";
        }
        else
        {
            networkManager.networkAddress = IPAddress.text;
        }

        //Port Setup
        if (joinPortNumber.text.CompareTo("") == 0)
        {
            Debug.Log("Empty Port, assigning 7777");
            networkManager.networkPort = 7777;
        }
        else
        {
            networkManager.networkPort = int.Parse(joinPortNumber.text);
        }

        Application.LoadLevel(1);
        networkManager.StartClient();
    }


    /*
     * Quit Functions
     */
    public void ConfirmQuit()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
    }

}
