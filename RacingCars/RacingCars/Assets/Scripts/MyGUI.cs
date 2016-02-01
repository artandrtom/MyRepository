using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class MyGUI : MonoBehaviour
{
    private string roomname = "RoomName";
    private string nickname = "NickName";
    private Hashtable properties;
    private bool isReady = false;
    private float posX = -19;
    private PhotonView myPhotonView;
    private Timer timer;
    private bool showboard = false;
    public Rect windowRect = new Rect(Screen.width-120, Screen.height-300, 240, 600);
    private List<string> table;
    private PlayFabManager myManager;
    private string bestPlayer="";
    private float bestValue;
    private int position = 0;
    void Start()
    {
        Application.runInBackground = true;
        GameObject.DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.ConnectUsingSettings("0.1");
        properties = new Hashtable();
        properties["ready"] = this.isReady;
        properties["finish"] = false;
        PhotonNetwork.player.SetCustomProperties(properties);
        myPhotonView = this.GetComponent<PhotonView>();
        timer = this.GetComponent<Timer>();
        timer.enabled = false;
        myManager = GetComponent<PlayFabManager>();
    }

    void OnGUI()
    {
        if (SceneManager.GetActiveScene().name.Equals("mainScene"))
        {
            if (!showboard)
            {
                return;
            }
            OnGUIBoard();
            return;
           
        }
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        switch (PhotonNetwork.connectionStateDetailed)
        {
            case PeerState.JoinedLobby:
                this.OnGUILobby();
                break;
            case PeerState.Joined:
                this.OnGUIJoined();
                break;
        }
    }

    private void OnGUIBoard()
    {
        windowRect = GUI.Window(0, new Rect(Screen.width/2 - 120, Screen.height/2 - 180, 240, 400), DoMyWindow, "Leaderboards");
    }
    void DoMyWindow(int windowID)
    {
        GUILayout.Label(bestPlayer+" wins by finishing in "+bestValue+" secends");
        GUILayout.Label("TOP 10 players:");
        for (int i=0; i<table.Count && i<10; i++)
        {
            GUILayout.Label((i+1)+") "+table.ToArray()[i].ToString()+" sec");         
        }
        GUILayout.Label(" Your place all time leaderboard: "+position.ToString());
        if (GUILayout.Button("Show and refresh Leaderboard"))
        {
            table = new List<string>(myManager.getStats());
            position = myManager.getPosition((float)(PhotonNetwork.player.customProperties["time"]));
        }
        if (GUILayout.Button("Return to Lobby"))
        {
            Destroy(GameObject.Find("Camera(Clone)"));
            Destroy(GameObject.Find("Car(Clone)"));
            PhotonNetwork.Disconnect();
            Destroy(gameObject);
            SceneManager.LoadScene("Lobby");
        }
    }
    private void OnGUITimer()
    {
        float time = Time.timeSinceLevelLoad;
        GUI.Box(new Rect(10, 10, 140, 30), string.Format("{0:0} min {1:00} sec", (int)(time / 60), (int)((time - (int)(time / 60) * 60))));
    }

    [PunRPC]
    void loadLevel()
    {
        SceneManager.LoadScene("mainScene");
        GameObject car = PhotonNetwork.Instantiate("Car", new Vector3((float)PhotonNetwork.player.customProperties["posX"], -5, 0), Quaternion.identity, 0);
        GameObject camera = Instantiate(Resources.Load("Camera"), Vector3.zero, Quaternion.identity) as GameObject;
        camera.AddComponent<CameraBehavior>();
        PhotonNetwork.player.TagObject = car;
        timer.enabled = true;
    }
   
    [PunRPC]
    void finish()
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if ((bool)player.customProperties["finish"] == false)
            {
                return;
            }
        }
        timer.enabled = false;
        if (PhotonNetwork.isMasterClient)
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {           
                myManager.SetUserData(player.name, (float)player.customProperties["time"]);               
            }
        }
        bestValue = (float)PhotonNetwork.player.customProperties["time"];
        bestPlayer = PhotonNetwork.player.name.ToString();
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            print((float)(player.customProperties["time"]));
            if((float)(player.customProperties["time"]) < bestValue)
            {
                bestValue = (float)(player.customProperties["time"]);
                bestPlayer = player.name.ToString();
            }
        }
        table = myManager.getStats();
        showboard = true;
    }
    
    private void OnGUILobby()
    {
        GUILayout.Label("Lobby Screen");
        GUILayout.Label(string.Format("Players in rooms: {0} looking for rooms: {1}  rooms: {2}", PhotonNetwork.countOfPlayersInRooms, PhotonNetwork.countOfPlayersOnMaster, PhotonNetwork.countOfRooms));
        this.roomname = GUILayout.TextField(this.roomname, GUILayout.Width(150));

        if (GUILayout.Button("Create Room", GUILayout.Width(150)))
        {
            PhotonNetwork.JoinOrCreateRoom(this.roomname, new RoomOptions() { maxPlayers = 4 }, TypedLobby.Default);
        }

        GUILayout.Label("Rooms to choose from: " + PhotonNetwork.countOfRooms);
        foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {
            if (GUILayout.Button(roomInfo.name))
            {
                PhotonNetwork.JoinRoom(roomInfo.name);
            }
        }
        if (GUILayout.Button("Back", GUILayout.Width(150)))
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Menu");
        }
    }
    private void OnGUIJoined()
    {
        GUILayout.Label("Room Screen. Players: " + PhotonNetwork.room.playerCount);
        GUILayout.Label("Room: " + PhotonNetwork.room.name + " Server: " + PhotonNetwork.ServerAddress);

        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            GUILayout.Label("Player Nickname: " + player.name + " Local: " + player.isLocal + " Master: " + player.isMasterClient + " Ready: " + player.customProperties["ready"]);
        }

        this.nickname = GUILayout.TextField(this.nickname, GUILayout.Width(150));
        if (GUILayout.Button("Set My NickName", GUILayout.Width(150)))
        {
            PhotonNetwork.player.name = this.nickname;        
        }
        if (GUILayout.Button("Ready to play", GUILayout.Width(150)))
        {
            properties = new Hashtable();
            this.isReady = !isReady;
            properties["ready"] = this.isReady;
            PhotonNetwork.player.SetCustomProperties(properties);
        }
        if (GUILayout.Button("Leave", GUILayout.Width(150)))
        {
            PhotonNetwork.LeaveRoom();
        }
        if (PhotonNetwork.player.isMasterClient)
        {
            if (GUILayout.Button("Start game", GUILayout.Width(150)))
            {
                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                    if(player.customProperties["ready"] == null)
                    {
                        return;
                    }
                    if (!player.customProperties["ready"].Equals(true)) return;
                }
                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                    properties = new Hashtable();
                    properties["posX"] = this.posX;
                    player.SetCustomProperties(properties);
                    this.posX += 2.5F;
                }
                this.myPhotonView.RPC("loadLevel", PhotonTargets.All);
            }
        }
        
    }
}
