using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {

    private Texture2D texure;
    private float velocity = 0.01F;

    // Use this for initialization
    void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
        move();
	}
    void move()
    {
        transform.Rotate(0,0,-3);
    }
}
