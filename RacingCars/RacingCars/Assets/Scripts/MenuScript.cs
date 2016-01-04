using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-80, 200, 60), "Play"))
        {
            Application.LoadLevel("mainScene");
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 60), "Multiplayer"))
        {
            Application.LoadLevel("lobby");
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 80, 200, 60), "Quit"))
        {
            Application.Quit();
        }
    }
}
