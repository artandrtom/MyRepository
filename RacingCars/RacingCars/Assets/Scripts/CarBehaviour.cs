﻿using UnityEngine;
using Photon;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class CarBehaviour : Photon.MonoBehaviour {
    private Rigidbody2D body;
    private movingDirection direction;
    private float drag;
    private float angleDrag;
    private Vector3 position;
    private  float speed;
    private PhotonView myPhotonView;
    private bool controlable = true;
    private Hashtable properties;
    void Start () {
        properties = new Hashtable();
        DontDestroyOnLoad(gameObject);
        body = GetComponent<Rigidbody2D>();
        drag = body.drag;
        angleDrag = body.angularDrag;
        position = body.position;
        myPhotonView = GetComponent<PhotonView>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!photonView.isMine)
        {
            return;
        }
        checkControls();
    }
    void checkControls()
    {
        if (!controlable)
        {
            return;
        }
        speed = getSpeed();
        if (Input.GetKey(KeyCode.W))
        {
            move(Vector3.up, 60);
            if (speed < 1)
            {
                direction = movingDirection.FORWARD;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            move(Vector3.down, 35);
            if (speed < 1)
            {
                direction = movingDirection.BACKWARD;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            turn(speed > 30 ? 30 : speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            turn(speed > 30 ? -30 : -speed);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            breaking();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.isMine)
        {
            return;
        }
        if (other.name.Equals("Finish"))
        {
            controlable = false;
            float time = Time.timeSinceLevelLoad;
            Hashtable properties = new Hashtable();
            properties["time"] = time;
            properties["finish"] = true;
            PhotonNetwork.player.SetCustomProperties(properties);
            PhotonView guiView = GameObject.Find("LobbyManager").GetComponent<PhotonView>();
            guiView.RPC("finish", PhotonTargets.All);  

        }
    }

    void move(Vector3 dir, float amount)
    {
        body.drag = getDrag();
        body.angularDrag = angleDrag;
        body.AddRelativeForce(dir * amount, ForceMode2D.Force);
    }

    float getDrag()
    {
        float x1 = body.transform.up.x;
        float y1 = body.transform.up.y;
        float x2 = body.velocity.x;
        float y2 = body.velocity.y;
        float numerator = x1 * x2 + y1 * y2;
        float denominator = (Mathf.Sqrt(Mathf.Pow(x1, 2) + Mathf.Pow(y1, 2)) * Mathf.Sqrt(Mathf.Pow(x2, 2) + Mathf.Pow(y2, 2)));
        if (denominator == 0)
        {
            return 0.5F;
        }
        float cos = numerator / denominator;
        float sin = Mathf.Sqrt(1 - Mathf.Pow(cos, 2));
        return double.IsNaN(0.5 + 3 * Mathf.Abs(sin)) ? 0 : (float)(0.5 + 3 * Mathf.Abs(sin));
    }


    float getSpeed()
    {        
        Vector3 newPosition = body.position;
        float path = Mathf.Sqrt(Mathf.Pow((newPosition.x-position.x),2) + Mathf.Pow((newPosition.y-position.y),2));
        position = newPosition;
        return path / Time.deltaTime;       

    }

    void turn(float torque)
    {
        if (direction == movingDirection.FORWARD)
        {
            body.AddTorque(-torque, ForceMode2D.Force);
        }
        else 
        {
            body.AddTorque(torque, ForceMode2D.Force);
        }
    }
    
    void breaking()
    {
        body.drag = 2.2F;
    }
}
public enum movingDirection
{
    FORWARD,
    BACKWARD,
}
