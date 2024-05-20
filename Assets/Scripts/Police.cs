using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Police : MonoBehaviour
{

    [SerializeField] private GameObject player;
    
    private NavMeshAgent _agent;
    private float _originalSpeed;
    private bool _chasingStarted;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _originalSpeed = _agent.speed;
        StartCoroutine(StartChasingPlayer());
    }

    private void Update()
    {
        
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

    private IEnumerator StartChasingPlayer()
    {
        yield return new WaitForSeconds(3f);
        _chasingStarted = true;

        while (_chasingStarted)
        {
            _agent.destination = player.transform.position;
            yield return new WaitForSeconds(1.5f);
        }
    }
}
