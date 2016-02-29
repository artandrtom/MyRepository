using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {	
	void Start () {
        GameObject joinLobby = GameObject.Find("JoinLobby");
        GameObject exit = GameObject.Find("Exit");
        EventDelegate.Set(joinLobby.GetComponent<UIButton>().onClick, () => SceneManager.LoadScene("lobby"));
        EventDelegate.Set(exit.GetComponent<UIButton>().onClick, () => Application.Quit());
    }
	
}
