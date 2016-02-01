using UnityEngine;
using System.Collections;

public class CameraBehavior : Photon.MonoBehaviour {
    Camera cam;
    GameObject car;
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        car = GameObject.Find("Car(Clone)");
        cam = GetComponent<Camera>();
        cam.enabled = true;
        cam.orthographicSize = 7.8F;
    }
    void Update()
    {
        if(car!=null)
        transform.position = new Vector3(car.transform.position.x, car.transform.position.y, -40);
    }

}
