using UnityEngine;
using System.Collections;

public class disableMouse : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        HideCursor();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HideCursor()
    {
        UnityEngine.Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        UnityEngine.Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
