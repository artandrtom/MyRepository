using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.LoadBalancing;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MyClient : MonoBehaviour {

    private LoadBalancingClient client;
    private string roomname = "RoomName";
    private string nickname = "NickName";
    private static List<LoadBalancingClient> clients;
    void Start () {
        Application.runInBackground = true;
        client = new LoadBalancingClient(null, "585750c50d7d4424b6606239998a33a9","1.0");
        client.ConnectToRegionMaster("eu");
        clients = new List<LoadBalancingClient>();
    }
    void Update()
    {
        if (client != null)
        {
            client.Service(); 
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
            switch (client.State)
            {
                case ClientState.JoinedLobby:
                    this.OnGUILobby();
                    break;
                case ClientState.Joined:
                    this.OnGUIJoined();
                    break;
            }
    }

    private void OnGUILobby()
    {
        GUILayout.Label("Lobby Screen");
        GUILayout.Label(string.Format("Players in rooms: {0} looking for rooms: {1}  rooms: {2}", this.client.PlayersInRoomsCount, this.client.PlayersOnMasterCount, this.client.RoomsCount));
        this.roomname = GUILayout.TextField(this.roomname, GUILayout.Width(150));

        if (GUILayout.Button("Create Room", GUILayout.Width(150)))
        {
            this.client.OpJoinOrCreateRoom(this.roomname, new RoomOptions(){MaxPlayers = 4},TypedLobby.Default);
        }

        GUILayout.Label("Rooms to choose from: " + this.client.RoomInfoList.Count);
        foreach (RoomInfo roomInfo in this.client.RoomInfoList.Values)
        {
            if (GUILayout.Button(roomInfo.Name))
            {
                this.client.OpJoinRoom(roomInfo.Name);
            }
        }
        if (GUILayout.Button("Back", GUILayout.Width(150)))
        {
            this.client.Disconnect();
            Application.LoadLevel("Menu");
        }
    }

    private void OnGUIJoined()
    {
        // we are in a room, so we can access CurrentRoom and it's Players
        
        GUILayout.Label("Room Screen. Players: " + this.client.CurrentRoom.Players.Count+ " Clients: "+clients.Count);
        GUILayout.Label("Room: " + this.client.CurrentRoom + " Server: " + this.client.CurrentServerAddress);

        foreach (Player player in this.client.CurrentRoom.Players.Values)
        {
            GUILayout.Label("Player: " + player + " Local: " + player.IsLocal + " Master: " + player.IsMasterClient);
        }

        this.nickname = GUILayout.TextField(this.nickname, GUILayout.Width(150));
        if (GUILayout.Button("Rename Self", GUILayout.Width(150)))
        {
            this.client.LocalPlayer.NickName = this.nickname;
        }
        if (GUILayout.Button("Ready to play", GUILayout.Width(150)))
        {
            
        }
        if (GUILayout.Button("Leave", GUILayout.Width(150)))
        {
            this.client.OpLeaveRoom();
        }
        if (GUILayout.Button("Appear Self(Test)", GUILayout.Width(150)))
        {
            /*
            foreach (Player player in this.client.CurrentRoom.Players.Values)
            {
               // GameObject Car = Instantiate(Resources.Load("Car")) as GameObject;
                if (player.IsLocal)
                {                  
                    player.Tag = Instantiate(Resources.Load("Car"));
                }
            }
            */
            this.client.LocalPlayer.Tag = Instantiate(Resources.Load("Car"));
        }
    }


    void OnApplicationQuit()
    {
        if (client != null) client.Disconnect();
        LoadBalancingPeer lbPeer = this.client.loadBalancingPeer;
        lbPeer.StopThread(); 
    }
 
}
