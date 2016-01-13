using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CameraBehavior : NetworkBehaviour {
    public Rigidbody2D body;
    //public Transform target;

    void Start()
    {
        body = GetComponentInParent<Rigidbody2D>();
    }
    void Update()
    {
        if (isLocalPlayer)
        {
            return;
        }
        transform.position = new Vector3(body.position.x, body.position.y+6,-10);     
    }

}
