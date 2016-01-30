using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    private float time;
	void Update () {
        time = Time.timeSinceLevelLoad;
	}
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 140, 30), string.Format("{0:0} min {1:00} sec", (int)(time / 60), (int)((time - (int)(time / 60) * 60))));
    }
}
