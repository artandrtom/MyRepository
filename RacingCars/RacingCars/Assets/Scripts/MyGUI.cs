using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class MyGUI : MonoBehaviour
{
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
        PhotonNetwork.player.name = "";
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
        GUILayout.Label(" Your place in all time leaderboard: "+position.ToString());
        if (GUILayout.Button("Show and refresh Leaderboard"))
        {
            refreshLeaderBoard();
        }
        if (GUILayout.Button("Return to Lobby"))
        {
            returnToLobby();
        }
    }
    private void refreshLeaderBoard()
    {
        GameObject[] labels = GameObject.FindGameObjectsWithTag("BoardLBS");
        foreach(var lab in labels)
        {
            Destroy(lab);
        }
        table = new List<string>(myManager.getStats());
        position = myManager.getPosition((float)(PhotonNetwork.player.customProperties["time"]));
        GameObject.Find("MainLabel").GetComponent<UILabel>().text = "Leaderboards\n";
        createLeaderLabel(bestPlayer + " wins by finishing in " + bestValue + " seconds" + "\n        \nTop 10 players: \n");
        for (int i = 0; i < table.Count && i < 10; i++)
        {
            createLeaderLabel((i + 1) + ") " + table.ToArray()[i].ToString() + " sec");
        }
        if(table.Count < 10)
        {
            for(int i=table.Count; i<=10; i++)
            {
                createLeaderLabel("");
            }
        }
        createLeaderLabel("\nYour place in all time leaderboard: " + position.ToString());
    }
    private void returnToLobby()
    {
        Destroy(GameObject.Find("Camera(Clone)"));
        Destroy(GameObject.Find(PhotonNetwork.player.name));
        PhotonNetwork.Disconnect();
        Destroy(gameObject);
        SceneManager.LoadScene("Lobby");
    }

    private void OnGUITimer()
    {
        float time = Time.timeSinceLevelLoad;
        GUI.Box(new Rect(10, 10, 140, 30), string.Format("{0:0} min {1:00} sec", (int)(time / 60), (int)((time - (int)(time / 60) * 60))));
    }


    void OnLevelWasLoaded(int level)
    {
        if (level == 2)
        {
            GameObject car = PhotonNetwork.Instantiate("Car", new Vector3((float)PhotonNetwork.player.customProperties["posX"], -5, 0), Quaternion.identity, 0);
            car.name = PhotonNetwork.player.name;
            GameObject camera = Instantiate(Resources.Load("Camera"), Vector3.zero, Quaternion.identity) as GameObject;
            camera.AddComponent<CameraBehavior>();
            timer.enabled = true;
        }
    }

    [PunRPC]
    void loadLevel()
    {
        SceneManager.LoadScene("mainScene");
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
        //showboard = true;
        GameObject.Find("SoundManager").SendMessage("playFinishMusic");
        GameObject.Find("UICamera").GetComponent<Camera>().enabled = true;
        GameObject button = GameObject.Find("RefreshButton");
        button.GetComponentInChildren<UILabel>().text = "Refresh Leaderboard";
        EventDelegate.Set(button.GetComponent<UIButton>().onClick, () => refreshLeaderBoard());
        button.name = "RefreshButton";

        button = GameObject.Find("BackButton");
        button.GetComponentInChildren<UILabel>().text = "Back to lobby";
        EventDelegate.Set(button.GetComponent<UIButton>().onClick, () => returnToLobby());
        button.name = "BackButton";
        refreshLeaderBoard();
    }

    void createLeaderLabel(string text)
    {
        GameObject label = GameObject.Find("MainLabel");
        label.GetComponent<UILabel>().text += "\n"+text;
    }
    private void readyToPlay()
    {
        properties = new Hashtable();
        this.isReady = !isReady;
        properties["ready"] = this.isReady;
        PhotonNetwork.player.SetCustomProperties(properties);
    }

    private void startGame()
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if ((player.customProperties["ready"] == null) || (player.name.Equals("")))
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
            this.posX += 2.65F;
        }
        this.myPhotonView.RPC("loadLevel", PhotonTargets.All);
    }
}
