using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.LoadBalancing;
public class MyClient : MonoBehaviour {

    private LoadBalancingClient client;

    void Start () {
        client = new LoadBalancingClient();
        client.AppId = "585750c5 - 0d7d - 4424 - b660 - 6239998a33a9";

        // "eu" is the European region's token
        bool connectInProcess = client.ConnectToRegionMaster("eu");
        MyCreateRoom("roomTest", 4);
        client.AutoJoinLobby = true;
       
    }

    void Update()
    {
        client.Service();
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
