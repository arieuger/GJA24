using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public Transform goal;
    private NavMeshAgent _agent;
    private float _originalSpeed;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _originalSpeed = _agent.speed;

        // agent.destination = goal.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            _agent.destination = mouseWorldPos;   
        }
        Debug.Log(_agent.speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Roads"))
        {
            _agent.speed = _originalSpeed * 1.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Roads"))
        {
            _agent.speed = _originalSpeed;
        }
    }
}
