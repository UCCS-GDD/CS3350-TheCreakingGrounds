using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvasHooks : MonoBehaviour
{
    public delegate void CanvasHook();

    public CanvasHook OnAddPlayerHook;
    public CanvasHook OnCharBuildHook;

    public Button firstButton;
    public Button secondButton;

    public GameObject charBuild;

    public void UIAddPlayer()
    {
        if (OnAddPlayerHook != null)
            OnAddPlayerHook.Invoke();
    }

    public void UICharacterBuild()
    {
        if (OnCharBuildHook != null)
            OnCharBuildHook.Invoke();

        charBuild.SetActive(true);
    }
}
