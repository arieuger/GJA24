using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private float _originalSpeed;
    private bool _isDestructing;
    [HideInInspector] public bool isBeingCharged;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _originalSpeed = _agent.speed;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isBeingCharged)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            _agent.destination = mouseWorldPos;   
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Roads"))
        {
            _agent.speed = _originalSpeed * 1.5f;
        } else if (other.tag.Equals("DestructionZone"))
        {
            if (!_isDestructing) other.gameObject.GetComponentInParent<Building>().StartDestruction();
            _isDestructing = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Roads"))
        {
            _agent.speed = _originalSpeed;
        } else if (other.tag.Equals("DestructionZone"))
        {
            if (_isDestructing) other.gameObject.GetComponentInParent<Building>().StopDestruction();
            _isDestructing = false;
        }
    }
}
