using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    
    public AudioClip menuClip;
    public AudioClip lobbyClip;
    public AudioClip mainClip;
    public AudioClip finishClip;
    void Start () {
        DontDestroyOnLoad(gameObject);
        AudioSource audio = GetComponent<AudioSource>();
    }
	
	void Update () {
	
	}
    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            GetComponent<AudioSource>().clip = menuClip;
            GetComponent<AudioSource>().Play();
        }
        if (level == 1)
        {
            GetComponent<AudioSource>().clip = lobbyClip;
            GetComponent<AudioSource>().Play();
        }
        if (level == 2)
        {
            GetComponent<AudioSource>().clip = mainClip;
            GetComponent<AudioSource>().Play();
        }
    }
    void playFinishMusic()
    {
        GetComponent<AudioSource>().clip = finishClip;
        GetComponent<AudioSource>().Play();
    }
}
