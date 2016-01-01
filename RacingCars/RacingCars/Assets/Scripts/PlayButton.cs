using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {

    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    private void OnMouseDown()
    {
        Application.LoadLevel("mainScene");
    }
}
