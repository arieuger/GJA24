using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Police : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private float chargeDuration;
    
    private NavMeshAgent _agent;
    private NavMeshAgent _playerAgent;
    private PlayerMovement _playerMovement;
    private float _originalSpeed;
    private bool _justCharged;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _originalSpeed = _agent.speed;

        _playerAgent = player.GetComponent<NavMeshAgent>();
        _playerMovement = player.GetComponent<PlayerMovement>();
        
        StartCoroutine(StartChasingPlayer());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Roads"))
        {
            _agent.speed = _originalSpeed * 1.5f;
        } 
        if (other.gameObject.tag.Equals("Player"))
        {
            if (!_justCharged)
            {
                _agent.ResetPath();
                _playerAgent.ResetPath();
                _justCharged = true;

                Vector3 positionOffset = transform.position - player.transform.position;
                positionOffset.z = 0;
                _playerMovement.isBeingCharged = true;

                Debug.DrawRay(transform.position, positionOffset.normalized * -5, Color.blue, Mathf.Infinity);
                player.GetComponent<Rigidbody2D>().AddForce(-positionOffset.normalized * 8f, ForceMode2D.Impulse);

                StartCoroutine(Discharge());
                
            }
        }
    }

    private IEnumerator Discharge()
    {
        float remainingTime = chargeDuration;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;            
        }
        
        _playerMovement.isBeingCharged = false;
        _justCharged = false;
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

        while (true)
        {
            if (!_justCharged)
            {
                _agent.destination = player.transform.position;
                yield return new WaitForSeconds(0.5f);
            }
            else yield return null;
        }
    }
}
