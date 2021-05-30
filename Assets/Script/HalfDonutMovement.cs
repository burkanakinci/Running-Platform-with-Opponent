using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfDonutMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0f;
    public bool move = false;
    private Vector3 targetPos;
    void Start()
    {
        targetPos = transform.position;
        Invoke("MoveStick", 3f);
    }
    void Update()
    {
        if (transform.localPosition.x <= 0)
        {
            move = false;
        }
        if (move)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,
                targetPos, (moveSpeed * 3) * Time.deltaTime);
        }
    }
    private void MoveStick()
    {
        float i = Random.Range(3f, 4f);
        move = true;
        Invoke("MoveStick", i);
    }
}
