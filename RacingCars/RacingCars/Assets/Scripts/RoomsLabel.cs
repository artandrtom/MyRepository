using UnityEngine;
using System.Collections;

public class RoomsLabel : MonoBehaviour {
    private UILabel label;

    void Start () {
        label = GetComponent<UILabel>();
	}
	
	void Update () {
        if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
        {
            label.text = "";
        }
        else
        {
            label.text = "Rooms to Join: " + PhotonNetwork.countOfRooms;
        }
    }
}
