using UnityEngine;
using UnityEngine.SceneManagement;
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
            SceneManager.LoadScene("mainScene");
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 60), "Multiplayer"))
        {
            SceneManager.LoadScene("lobby");
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 80, 200, 60), "Quit"))
        {
            Application.Quit();
        }
    }
}
