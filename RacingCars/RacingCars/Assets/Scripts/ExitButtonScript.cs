using UnityEngine;
using System.Collections;

public class ExitButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void OnMouseDown()
    {
        Application.Quit();
        Debug.Log("quiting");
    }
}
