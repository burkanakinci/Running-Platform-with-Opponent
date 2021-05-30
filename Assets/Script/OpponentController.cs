using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System.Net.Cache;
using System.Dynamic;

public class OpponentController : Agent
{
    [SerializeField] private float runSpeed = 5f;
    public float lateral = 0.1f;
    private KeyCode leftKey = KeyCode.LeftArrow;
    private KeyCode RightKey = KeyCode.RightArrow;
    private Vector3 startingPos;
    [SerializeField] private Rigidbody rbOpponent;
    private bool onRotating = false;

    public event Action OnReset;
    public override void Initialize()
    {
        startingPos = transform.position;
    }
    void FixedUpdate()
    {

        RequestDecision();
        if (onRotating)
        {
            rbOpponent.MovePosition(new Vector3(transform.position.x, transform.position.y,
                transform.position.z + runSpeed * Time.fixedDeltaTime));
            MoveRight();
        }
        else
        {
            rbOpponent.MovePosition(new Vector3(transform.position.x, transform.position.y,
                transform.position.z + runSpeed * Time.fixedDeltaTime));
        }
        if (transform.position.y <= -2)
        {
            AddReward(increment: -1.0f);
            transform.position = startingPos;
        }
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
            MoveLeft();
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
            MoveRight();
        }
    }
    public override void OnEpisodeBegin()
    {
        Reset();
    }
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        actionsOut[1] = 0;
        if (Input.GetKey(leftKey))
        {
            MoveLeft();
            actionsOut[0] = 1;
        }
        if (Input.GetKey(RightKey))
        {
            MoveRight();
            actionsOut[1] = 1;
        }
    }
    private void MoveLeft()
    {
        Vector3 pos = transform.position;
        pos.x -= lateral;
        transform.position = pos;
    }
    private void MoveRight()
    {
        Vector3 pos = transform.position;
        pos.x += lateral;
        transform.position = pos;
    }

    private void Reset()
    {
        transform.position = startingPos;
        OnReset?.Invoke();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "StaticObs" || other.gameObject.tag == "HorizontalObs")
        {
            AddReward(increment: -1.0f);
            EndEpisode();
        }
        if (other.gameObject.tag == "RotatingPlatform")
        {
            onRotating = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {

        if (other.gameObject.tag == "RotatingPlatform")
        {
            onRotating = false;
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "RotatingPlatform")
        {
            onRotating = true;
            if (transform.localPosition.x <= 0)
            {
                AddReward(increment: -1.0f);
            }
        }
    }
}
