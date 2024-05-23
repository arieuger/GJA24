using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float timeToContinuousCharge = 15f;
    [SerializeField] private float blockTime = 2f;
    
    [HideInInspector] public bool isBeingCharged;
    [HideInInspector] public int continuousChargeCount;
    [HideInInspector] public bool isChargeCounting;
    [HideInInspector] public bool isBlocked;
    [HideInInspector] public bool isInEscapeGrace;

    private NavMeshAgent _agent;
    private float _originalSpeed;
    private bool _isDestructing;
    private Color _originalColor;
    private bool _isFlipped;
    
    public static PlayerMovement Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _originalSpeed = _agent.speed;
        _originalColor = GetComponent<SpriteRenderer>().color;

        StartCoroutine(RemoveBlockPoint());
    }
    
    private void Update()
    {

        if (!GameManager.Playing) return;
        
        if (Input.GetMouseButtonDown(0) && !isBeingCharged && !isBlocked)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            _agent.destination = mouseWorldPos;   
        }

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

    public void Block()
    {
        StartCoroutine(BlockCo());
    }

    public IEnumerator BlockCo()
    {
        isBlocked = true;
        continuousChargeCount = 0;
        FindObjectsByType<Building>(FindObjectsSortMode.None).ToList().ForEach(b => b.StopDestruction());
        float remainingTime = blockTime;
        while (remainingTime > 0f)
        {
            float lerp = Mathf.PingPong(Time.time, 0.25f);
            GetComponent<SpriteRenderer>().color = Color.Lerp(_originalColor, Color.red, lerp);
            
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = _originalColor;
        isChargeCounting = false;
        isBlocked = false;
        isInEscapeGrace = true;
        remainingTime = 1f;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        isInEscapeGrace = false;
    }

    public IEnumerator RemoveBlockPoint()
    {
        while (true)
        {
            if (continuousChargeCount is > 0 and <= 3)
            {
                continuousChargeCount--;
            }
            yield return new WaitForSeconds(timeToContinuousCharge / 3);
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
