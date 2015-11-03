using UnityEngine;
using System.Collections;

public class disableMouse : MonoBehaviour {

    CursorLockMode wantedMode;

	// Use this for initialization
	void Start () {
        //Set cursor to lock in the center
        wantedMode = CursorLockMode.Locked;

        //Set cursor to wantedMode
        Cursor.lockState = wantedMode;

        //Don't show the mouse
		UnityEngine.Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
