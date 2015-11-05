using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
//using System.Linq;
//using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    private GameObject MainMenuCanvas;
    private GameObject OptionsGameCanvas;
    private GameObject QuitGameCanvas;
    private GameObject MessageCanvas;
    private Button BackButton;

    void Start()
    {
        //Find objects
        MainMenuCanvas = GameObject.Find("MainMenu");
        MessageCanvas = GameObject.Find("GameMessage");
        OptionsGameCanvas = GameObject.Find("OptionsGame");
        QuitGameCanvas = GameObject.Find("QuitGame");
        BackButton = GameObject.Find("BackButton").GetComponent<Button>();

        //Setup objects
        BackButton.gameObject.SetActive(false);
        MainMenuShow();
    }

    /*
     *Main Menu button functions
     */

    public void PlayGame()
    {
        //Go to lobby to start playing and setting up game
        Application.LoadLevel(1);
    }

    public void LinkStart()
    {
        Application.OpenURL("www.edengrounds.net/TheCreakingGrounds");
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
        MessageCanvas.SetActive(false);
        QuitGameCanvas.SetActive(true);
        //BackButton.gameObject.SetActive(true);
    }

    //Used by back button primarily
    public void MainMenuShow()
    {
        OptionsGameCanvas.SetActive(false);
        QuitGameCanvas.SetActive(false);
        BackButton.gameObject.SetActive(false);

        MainMenuCanvas.SetActive(true);
        MessageCanvas.SetActive(true);
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
