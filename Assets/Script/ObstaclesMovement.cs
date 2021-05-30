using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool direction = false;
    void Start()
    {

    }

    void FixedUpdate()
    {

        if (direction)
        {
           transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name=="leftCollider"||other.gameObject.name=="rightCollider"){
            
            direction=!direction;
        }
    }
}
