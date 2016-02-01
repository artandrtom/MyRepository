using UnityEngine;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayFabManager : MonoBehaviour {
    public string PlayFabId;
    private Hashtable properties;
    private List<string> numbers;
    void Start () {
        PlayFabSettings.TitleId = "AF33";
        //PlayFabSettings.DeveloperSecretKey = "PGHRFTUNZSUKCZ38NITBGC98KXGKF65NUFITMIIYZ6FX7JKEBD";
        Login("AF33");
        properties = new Hashtable();
        numbers = new List<string>();
	}
	
    void Login(string titleId)
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = titleId,
            CreateAccount = true,
            CustomId = "RacingCars"
            //SystemInfo.deviceUniqueIdentifier
        };

        PlayFabClientAPI.LoginWithCustomID(request, (result) => {
            PlayFabId = result.PlayFabId;
            Debug.Log("Got PlayFabID: " + PlayFabId);

            if (result.NewlyCreated)
            {
                Debug.Log("(new account)");
            }
            else
            {
                Debug.Log("(existing account)");
            }
        },
        (error) => {
            Debug.Log("Error logging in player with custom ID:");
            Debug.Log(error.ErrorMessage);
        });
    }
    
    public void SetUserData(string name, float time)
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string,string>(){
      {name,time.ToString()}
    }
        };

        PlayFabClientAPI.UpdateUserData(request, (result) =>
        {
            Debug.Log("Successfully updated user data for "+ name);
        }, (error) =>
        {
            Debug.Log("Got error setting user data "+time+" to "+name);
            Debug.Log(error.ErrorDetails);
        });
    }
    
    public List<string> getStats()
    {
        GetUserData();
        List < string > table = new List<string>(sort(properties));
        return table;
    }
    public int getPosition(float num)
    {
            return numbers.IndexOf(num.ToString())+1;
    }
    private List<string> sort(Hashtable properties)
    {
        numbers = new List<string>();
        List<float> list = new List<float>();
        foreach (var item in properties)
        {
            if(item.Value!=null)
            list.Add(System.Convert.ToSingle((string)(item.Value)));
        }
        list.Sort();
        List<string> table = new List<string>();
        foreach(float number in list)
        {
            numbers.Add(number.ToString());
            foreach (var item in properties)
            {               
                if(number == System.Convert.ToSingle((string)(item.Value)))
                {
                    table.Add(item.Key.ToString() + " : " + number.ToString());
                }
            }
        }
        return table;
    }
    public void GetUserData()
    {
        GetUserDataRequest request = new GetUserDataRequest()
        {
            PlayFabId = PlayFabId,
            Keys = null
        };

        PlayFabClientAPI.GetUserData(request, (result) => {
            Debug.Log("Got user data:");
            if ((result.Data == null) || (result.Data.Count == 0))
            {
                Debug.Log("No user data available");
            }
            else
            {
                properties = new Hashtable();
                foreach (var item in result.Data)
                {
                    Debug.Log("    " + item.Key + " == " + item.Value.Value);
                    properties.Add(item.Key, item.Value.Value);
                }
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.ErrorMessage);
        });
    }

}
