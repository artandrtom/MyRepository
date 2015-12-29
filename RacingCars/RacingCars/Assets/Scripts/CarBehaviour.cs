using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {
    public Rigidbody2D body;
    public movingDirection direction;
    public float drag;
    public float angleDrag;
    public Vector2 position;
    public  float speed;
    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        drag = body.drag;
        angleDrag = body.angularDrag;
        position = body.position;
    }
	
	// Update is called once per frame
	void Update () {
        speed = getSpeed();

        if (Input.GetKey(KeyCode.W))
        {
            move(Vector2.down, 60);
            if (speed < 1)
            {
                direction = movingDirection.FORWARD;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            move(Vector2.up, 35);
            if (speed < 1)
            {
                direction = movingDirection.BACKWARD;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            turn(speed%30);
        }
        if (Input.GetKey(KeyCode.A))
        {
            turn(-speed%30);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            breaking();
        }
	}

    void move(Vector2 dir, float amount)
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
        return (float)(0.5 + 3 * Mathf.Abs(sin));
    }


    float getSpeed()
    {
        Vector2 newPosition = body.position;
        float path = Mathf.Sqrt(Mathf.Pow((newPosition.x-position.x),2) + Mathf.Pow((newPosition.y-position.y),2));
        position = newPosition;
        return path / Time.deltaTime;
        //return body.velocity.magnitude / (Time.fixedDeltaTime*40);

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
        //body.angularDrag = 5;
    }
}
public enum movingDirection
{
    FORWARD,
    BACKWARD,
}
