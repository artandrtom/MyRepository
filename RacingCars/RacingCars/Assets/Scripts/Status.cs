using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Status : MonoBehaviour {
    private UILabel label;
    private int posY = -100;
	void Start () {
        label = this.GetComponent<UILabel>();
    }
	void Update () {
        label.text = PhotonNetwork.connectionStateDetailed.ToString();
        if(PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby)
        {
            label.text += "\nLobby Screen \n"+ string.Format("Players in rooms: {0} \nlooking for rooms: {1}  \nrooms: {2}", PhotonNetwork.countOfPlayersInRooms, PhotonNetwork.countOfPlayersOnMaster, PhotonNetwork.countOfRooms);
            if (GameObject.Find("RoomButtons").transform.childCount != PhotonNetwork.countOfRooms)
            {
                drawRoomButtons();
            }
        }
        if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
        {
            label.text += "\nRoom Screen. Players: " + PhotonNetwork.room.playerCount + "/4"+"\nRoom: "+ PhotonNetwork.room.name;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                label.text+= "\nNickname: " + player.name + (player.customProperties["ready"].Equals(true) ? ": ready" : ": not ready");
            }           
        }
    }
    void OnJoinedLobby()
    {
        GameObject button = Resources.Load("BaseButton") as GameObject;
        button.GetComponentInChildren<UILabel>().text = "Create Room";
        EventDelegate.Set(button.GetComponent<UIButton>().onClick, () => myCreateRoom());
        button = NGUITools.AddChild(GameObject.Find("Buttons"), button);
        button.transform.localPosition = new Vector3(0, -90, 0);
        button.name = "CreateRoomButton";

        button = Resources.Load("BaseButton") as GameObject;
        button.GetComponentInChildren<UILabel>().text = "Back to menu";
        EventDelegate.Set(button.GetComponent<UIButton>().onClick, () => loadMenu());
        button = NGUITools.AddChild(GameObject.Find("Buttons"), button);
        button.transform.localPosition = new Vector3(0, -390, 0);
        button.name = "BackButton";
    }

    void drawRoomButtons()
    {                 
        print("rooms: " + PhotonNetwork.countOfRooms);
        foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {            
            setButton(roomInfo.name.ToString());          
        }
        if (GameObject.Find("RoomButtons").transform.childCount != PhotonNetwork.countOfRooms)
        {
            destroyRoomBtns(GameObject.FindGameObjectsWithTag("RoomBTS"));
        }
        posY = -100;
    }
    void destroyRoomBtns(GameObject[] roombuttons)
    {
        foreach (var roomBtn in roombuttons)
        {
            Destroy(roomBtn);
        }
    }
    void myJoinRoom(string roomName)
    {
        print("joining room: " + roomName);
        PhotonNetwork.JoinRoom(roomName);
    }
    void setButton(string roomName)
    {
        GameObject roombButton = Resources.Load("RoomButton") as GameObject;       
        roombButton.GetComponentInChildren<UILabel>().text = roomName;
        print("setting action to button:" + roombButton.GetComponentInChildren<UILabel>().text);
        roombButton = NGUITools.AddChild(GameObject.Find("RoomButtons"), roombButton);
        
        roombButton.transform.localPosition = new Vector3(0, posY, 0);
        roombButton.name = roomName;
        EventDelegate.Set(roombButton.GetComponent<UIButton>().onClick, () => myJoinRoom(roombButton.GetComponentInChildren<UILabel>().text.ToString()));
        posY -= 50;
    }
    void OnJoinedRoom()
    {
        destroyRoomBtns(GameObject.FindGameObjectsWithTag("RoomBTS"));
        destroyRoomBtns(GameObject.FindGameObjectsWithTag("BaseBTS"));


        GameObject button = Resources.Load("BaseButton") as GameObject;
        button.GetComponentInChildren<UILabel>().text = "Leave Room";
        EventDelegate.Set(button.GetComponent<UIButton>().onClick, () => myLeaveRoom());
        button = NGUITools.AddChild(GameObject.Find("Buttons"), button);
        button.transform.localPosition = new Vector3(0, -390, 0);
        button.name = "LeaveRoomButton";

        button = Resources.Load("BaseButton") as GameObject;
        button.GetComponentInChildren<UILabel>().text = "Set Nickname";
        EventDelegate.Set(button.GetComponent<UIButton>().onClick, () => setNickname());
        button = NGUITools.AddChild(GameObject.Find("Buttons"), button);
        button.transform.localPosition = new Vector3(0, -90, 0);
        button.name = "SetNicknameButton";

        button = Resources.Load("BaseButton") as GameObject;
        button.GetComponentInChildren<UILabel>().text = "Ready to play";
        EventDelegate.Set(button.GetComponent<UIButton>().onClick, () => readyToPlay());
        button = NGUITools.AddChild(GameObject.Find("Buttons"), button);
        button.transform.localPosition = new Vector3(0, -140, 0);
        button.name = "ReadyToPlayButton";

        button = Resources.Load("BaseButton") as GameObject;
        button.GetComponentInChildren<UILabel>().text = "Start Game";
        EventDelegate.Set(button.GetComponent<UIButton>().onClick, () => startGame());
        button = NGUITools.AddChild(GameObject.Find("Buttons"), button);
        button.transform.localPosition = new Vector3(0, -190, 0);
        button.name = "StartGameButton";
    }
    void startGame()
    {
        GameObject.Find("LobbyManager").GetComponent<MyGUI>().SendMessage("startGame");
    }
    void readyToPlay()
    {
        GameObject.Find("LobbyManager").GetComponent<MyGUI>().SendMessage("readyToPlay");
    }

    void loadMenu()
    {
        PhotonNetwork.Disconnect();
        Destroy(GameObject.Find("LobbyManager"));
        SceneManager.LoadScene("Menu");
    }
    void setNickname()
    {
        string nickName = GameObject.Find("Input").GetComponentInChildren<UILabel>().text;
        PhotonNetwork.player.name = nickName;
    }
    void myCreateRoom()
    {
        string roomName = GameObject.Find("Input").GetComponentInChildren<UILabel>().text;
        if (roomName.Length == 0)
        {
            return;
        }
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { maxPlayers = 4 }, TypedLobby.Default);
    }
    void myLeaveRoom()
    {
        destroyRoomBtns(GameObject.FindGameObjectsWithTag("BaseBTS"));
        PhotonNetwork.LeaveRoom();
    }
}
