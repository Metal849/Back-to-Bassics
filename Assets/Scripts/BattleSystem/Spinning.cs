using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float maxSpeed;
    public float speed;
    public bool ccw;
    private float speedUp;
    private const float accelerate = 0.05f;
    private float halfMaxSpeed;

    private void Start()
    {
        halfMaxSpeed = maxSpeed / 2f;
    }

    private void FixedUpdate()
    {
        if (speedUp < speed)
        {
            speedUp += accelerate;
        }
        else
        {
            speedUp = speed;
        }
        Quaternion rotateTo = transform.rotation * Quaternion.AngleAxis((ccw ? -1 : 1) * 90, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotateTo, Time.deltaTime * speedUp);
    }
    public void ChangeDirection(float newSpeedUp)
    {
        speedUp = newSpeedUp;
        ccw = !ccw;
    }

    public void ChangeDirectionRandomSpeed()
    {
        speedUp = Random.Range(0f, halfMaxSpeed);
        speed = Random.Range(halfMaxSpeed, maxSpeed);
        ccw = !ccw;
    }
}
