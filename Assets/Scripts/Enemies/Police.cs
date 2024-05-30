using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Police : MonoBehaviour
{

    [SerializeField] private float chargeDuration;
    [SerializeField] private float chargeForce = 16f;
    
    private NavMeshAgent _agent;
    private NavMeshAgent _playerAgent;
    private PlayerMovement _playerMovement;
    private Animator _animator;
    private float _originalSpeed;
    private bool _justCharged;
    private bool _chargedAndExited;
    private bool _isFlipped;
    private bool _isEndingGame;

    // SOUNDS
    [SerializeField] private AudioSource sirenSound;
    [SerializeField] private AudioSource chargeSound;
    
    // Anim parameters
    private static readonly int Movement = Animator.StringToHash("movement");
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _animator = GetComponent<Animator>();

        _originalSpeed = _agent.speed;

        _playerAgent = PlayerMovement.Instance.GetComponent<NavMeshAgent>();
        _playerMovement = PlayerMovement.Instance;

        sirenSound.Play();
        
        StartCoroutine(StartChasingPlayer());
    }

    private void Update()
    {
        
        _animator.SetFloat(Movement, _agent.velocity.magnitude);
        
        if (_agent.remainingDistance >= 0.1f)
        {
            Vector3 vectorToTarget = _agent.destination - transform.position;
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            transform.rotation = targetRotation;

            if (_agent.destination.x < transform.position.x && !_isFlipped)
            {
                _isFlipped = true;
                var localScale = transform.localScale;
                localScale.y = Math.Abs(localScale.y) * -1;
                transform.localScale = localScale;
            } else if (_agent.destination.x > transform.position.x && _isFlipped)
            {
                _isFlipped = false;
                var localScale = transform.localScale;
                localScale.y = Math.Abs(localScale.y);
                transform.localScale = localScale;
            }    
        }
    }
    
    public IEnumerator PoliceEndGame()
    {
        _isEndingGame = true;
        _agent.ResetPath();
        
        while (_agent.speed > 0f)
        {
            _agent.speed -= 0.5f * Time.deltaTime; 
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isEndingGame && other.tag.Equals("Roads"))
        {
            _agent.speed = _originalSpeed * 1.5f;
        } 
        if (other.gameObject.tag.Equals("Player"))
        {
            if (!_isEndingGame && !_justCharged)
            {
                ChargeAgainstPlayer();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_isEndingGame && other.tag.Equals("Roads"))
        {
            _agent.speed = _originalSpeed;
        }
    }

    private void ChargeAgainstPlayer()
    {
        _agent.ResetPath();
        _playerAgent.ResetPath();
        _justCharged = true;

        chargeSound.Play();

        Vector3 positionOffset = transform.position - PlayerMovement.Instance.transform.position;
        positionOffset.z = 0;
        _playerMovement.isBeingCharged = true;
                
        PlayerMovement.Instance.GetComponent<Rigidbody2D>().AddForce(-positionOffset.normalized * chargeForce, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddForce(positionOffset * 2.5f, ForceMode2D.Impulse);

        if (!_playerMovement.isInEscapeGrace) _playerMovement.AddChargeCount();
        if (_playerMovement.continuousChargeCount >= 3 && !_playerMovement.isBlocked && !_playerMovement.isInEscapeGrace)
        {
            _playerMovement.Block();
        }

        StartCoroutine(Discharge());
    }

    private IEnumerator Discharge()
    {
        float remainingTime = chargeDuration;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;            
        }

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PlayerMovement.Instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        _playerMovement.isBeingCharged = false;
        _justCharged = false;
    }

    private IEnumerator StartChasingPlayer()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {

            if (Random.value > 0.85f /*&& FindObjectsByType<Police>(FindObjectsSortMode.None).Length > 1*/)
            {
                _agent.destination = FindRandomPoint();
                yield return new WaitForSeconds(3f);
            }
            
            if (!_isEndingGame && !_justCharged && !_playerMovement.isBlocked && !_playerMovement.isInEscapeGrace)
            {
                _agent.destination = PlayerMovement.Instance.transform.position;
                yield return new WaitForSeconds(0.5f);
            }
            else yield return null;
        }
    }

    private Vector2 FindRandomPoint()
    {
        float y = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, 
            Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float x = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, 
            Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        return new Vector2(x, y);
    }
    
}
