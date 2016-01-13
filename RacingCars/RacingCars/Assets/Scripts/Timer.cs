using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    float time;
	void Update () {
        time = Time.timeSinceLevelLoad;
	}
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 140, 30), time.ToString());
    }
}
