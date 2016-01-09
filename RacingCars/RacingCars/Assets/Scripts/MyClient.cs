using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.LoadBalancing;
public class MyClient : MonoBehaviour {

    private LoadBalancingClient client;
    private Room room;
    private Player player;
    string nickname = "name";
    void Start () {
        client = new LoadBalancingClient();
        client.AppId = "585750c50d7d4424b6606239998a33a9";

        // "eu" is the European region's token
        bool connectInProcess = client.ConnectToRegionMaster("eu");
        room = client.CreateRoom("testRoom", new RoomOptions() { MaxPlayers = 4 });
        client.AutoJoinLobby = true;
        Debug.Log(connectInProcess);
        Debug.Log(client.PlayersInRoomsCount+" "+client.PlayersOnMasterCount);
        Debug.Log(client.CurrentServerAddress);
        Debug.Log(client.MasterServerAddress);
        Debug.Log(client.NameServerAddress);
        Debug.Log("rooms: " + client.RoomsCount);
        player = new Player(nickname, 1, true);
    }

    void Update()
    {
        client.Service();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(100,50,300,150),room.ToStringFull());
        GUI.TextField(new Rect(150, 150, 100, 40), nickname);
        if(GUI.Button(new Rect(150, 200, 100, 50), "Join"))
        {
            room.AddPlayer(player);
            Debug.Log(room.ToStringFull());
        }
    }

    void OnApplicationQuit()
    {
        client.Disconnect();
    }
    void MyCreateRoom(string roomName, byte maxPlayers)
    {
        client.OpCreateRoom(roomName, new RoomOptions() { MaxPlayers = maxPlayers }, TypedLobby.Default);
    }
   
}
