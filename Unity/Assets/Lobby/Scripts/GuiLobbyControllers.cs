using System;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

[Serializable]
public class CanvasControl
{
    [SerializeField]
    public Canvas prefab;
    Canvas m_Canvas;

    public Canvas canvas { get { return m_Canvas; } }

    public virtual void Show()
    {
        if (prefab == null)
            return;

        if (m_Canvas != null)
            return;

        m_Canvas = (Canvas)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        GameObject.DontDestroyOnLoad(m_Canvas.gameObject);
    }

    public void Hide()
    {
        if (m_Canvas == null)
            return;

        GameObject.Destroy(m_Canvas.gameObject);
        m_Canvas = null;
    }

    public virtual void OnLevelWasLoaded()
    {
    }
}

[Serializable]
public class LobbyCanvasControl : CanvasControl
{
    public override void Show()
    {
        base.Show();

        var hooks = canvas.GetComponent<LobbyCanvasHooks>();
        if (hooks == null)
            return;

        hooks.OnAddPlayerHook = OnGUIAddPlayer;
    }

    public void OnGUIAddPlayer()
    {
        GuiLobbyManager.s_Singleton.popupCanvas.Hide();

        int id = NetworkClient.allClients[0].connection.playerControllers.Count;
        ClientScene.AddPlayer((short)id);
    }

    public void SetFocusToAddPlayerButton()
    {
        var hooks = canvas.GetComponent<LobbyCanvasHooks>();
        if (hooks == null)
            return;

        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }
}

[Serializable]
public class OnlineCanvasControl : CanvasControl
{
    public void Show(string status)
    {
        base.Show();

        GuiLobbyManager.s_Singleton.offlineCanvas.Hide();

        var hooks = canvas.GetComponent<OnlineControlHooks>();
        if (hooks == null)
            return;

        hooks.OnStopHook = OnGUIStop;

        hooks.SetAddress(GuiLobbyManager.s_Singleton.networkAddress);
        hooks.SetStatus(status);

        GuiLobbyManager.s_Singleton.onlineStatus = status;

        EventSystem.current.firstSelectedGameObject = hooks.firstButton.gameObject;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public void OnGUIStop()
    {
        GuiLobbyManager.s_Singleton.popupCanvas.Hide();
        GuiLobbyManager.s_Singleton.StopHost();
    }
}

[Serializable]
public class OfflineCanvasControl : CanvasControl
{
    public override void Show()
    {
        base.Show();
        GuiLobbyManager.s_Singleton.onlineCanvas.Hide();

        var hooks = canvas.GetComponent<OfflineControlHooks>();
        if (hooks == null)
            return;

        hooks.OnStartHostHook = OnGUIStartHost;
        hooks.OnStartServerHook = OnGUIStartServer;
        hooks.OnStartClientHook = OnGUIStartClient;

        EventSystem.current.firstSelectedGameObject = hooks.firstButton.gameObject;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public override void OnLevelWasLoaded()
    {
        if (canvas == null)
            return;

        var hooks = canvas.GetComponent<OfflineControlHooks>();
        if (hooks == null)
            return;

        EventSystem.current.firstSelectedGameObject = hooks.firstButton.gameObject;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public void OnGUIStartHost()
    {
        GuiLobbyManager.s_Singleton.StartHost();
        GuiLobbyManager.s_Singleton.onlineCanvas.Show("Host");
    }

    public void OnGUIStartServer()
    {
        GuiLobbyManager.s_Singleton.StartServer();
        GuiLobbyManager.s_Singleton.onlineCanvas.Show("Server");
    }

    public void OnGUIStartClient()
    {
        var hooks = canvas.GetComponent<OfflineControlHooks>();
        if (hooks == null)
            return;

        GuiLobbyManager.s_Singleton.networkAddress = hooks.GetAddress();
        GuiLobbyManager.s_Singleton.StartClient();
        GuiLobbyManager.s_Singleton.onlineCanvas.Show("Client");
    }
}

[Serializable]
public class ConnectingCanvasControl : CanvasControl
{
    public void Show(string address)
    {
        base.Show();


        var hooks = canvas.GetComponent<ConnectingCanvasHooks>();
        if (hooks == null)
            return;

        hooks.OnExitHook = OnGUICancelConnecting;

        hooks.messagText.text = address;
        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public void OnGUICancelConnecting()
    {
        Hide();
        GuiLobbyManager.s_Singleton.StopHost();
    }
}

[Serializable]
public class PopupCanvasControl : CanvasControl
{
    public void Show(string title, string message)
    {
        base.Show();

        var hooks = canvas.GetComponent<PopupMessageHooks>();
        if (hooks == null)
            return;

        hooks.OnExitHook = OnGUIExitPopup;

        hooks.titleText.text = title;
        hooks.messagText.text = message;

        EventSystem.current.SetSelectedGameObject(hooks.firstButton.gameObject);
    }

    public void OnGUIExitPopup()
    {
        Hide();
    }
}