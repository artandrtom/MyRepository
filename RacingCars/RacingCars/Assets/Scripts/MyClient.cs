using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.LoadBalancing;
public class MyClient : MonoBehaviour {

    private LoadBalancingClient client;
    private Room room;
    private Player player;
    string nickname = "Pidor#";
    void Start () {
        client = new LoadBalancingClient(null, "585750c50d7d4424b6606239998a33a9","1.0");
        client.ConnectToRegionMaster("eu");
        client.AutoJoinLobby = false;
        room = new Room();
        player = new Player(nickname + (int)(Random.value * 100), (int)(Random.value * 100), true);
    }

    void Update()
    {
        if (client != null)
        {
            client.Service();  // easy but ineffective. should be refined to using dispatch every frame and sendoutgoing on demand
        }
    }

    void OnGUI()
    {
        if (client != null)
        {
            GUILayout.Label(client.State + " " + client.Server+ room.ToString() + " nick: " + player.NickName+" players: "+room.PlayerCount);
            if (GUI.Button(new Rect(150, 200, 100, 50), "Join or Create"))
            {

                room = new Room("MyRoom", new RoomOptions() { MaxPlayers = 3 });
                client.CurrentRoom = room;               
                room.AddPlayer(player);
                //room.StorePlayer(player);
                Debug.Log(room.PlayerCount);
                Debug.Log(room.IsLocalClientInside);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (client != null) client.Disconnect();
    }
    void MyCreateRoom(string roomName, byte maxPlayers)
    {
        client.OpJoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = maxPlayers }, TypedLobby.Default);
    }
   
}
