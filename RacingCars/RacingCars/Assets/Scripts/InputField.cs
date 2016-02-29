using UnityEngine;
using System.Collections;

public class InputField : MonoBehaviour {
    private UILabel label;
    void Start()
    {
        label = this.GetComponentInChildren<UILabel>();
    }
    void OnJoinedLobby()
    {
        label.text = "Enter room name here";
    }
    void OnJoinedRoom()
    {
        label.text = "Enter your nickname";
    }
}
