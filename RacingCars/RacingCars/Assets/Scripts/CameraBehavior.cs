using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {
    public Transform target;
    float time;
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y+6,-10);
        time = Time.realtimeSinceStartup;
        
    }
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 140, 30), time.ToString());
    }
}
