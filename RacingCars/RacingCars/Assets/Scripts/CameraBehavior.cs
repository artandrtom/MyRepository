using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {
    public Transform target;
   

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y+6,-10);
    }
}
