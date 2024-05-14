using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private float _originalSpeed;
    private bool _isDestructing;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _originalSpeed = _agent.speed;
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
        // Debug.Log(_isDestructing);
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
