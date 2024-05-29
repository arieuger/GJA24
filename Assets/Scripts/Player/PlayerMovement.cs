using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float timeToContinuousCharge = 15f;
    [SerializeField] private float blockTime = 2f;
    [SerializeField] private List<Image> lightImages;
    [SerializeField] private Sprite redLight;
    [SerializeField] private Sprite yellowLight;
    // SOUNDS
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource blockedSound;
    
    [HideInInspector] public bool isBeingCharged;
    [HideInInspector] public int continuousChargeCount;
    [HideInInspector] public bool isBlocked;
    [HideInInspector] public bool isInEscapeGrace;

    private NavMeshAgent _agent;
    private Animator _animator;
    private float _originalSpeed;
    private Color _originalColor;
    private bool _isFlipped;
    
    // Anim parameters
    private static readonly int Movement = Animator.StringToHash("movement");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

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

        _animator = GetComponent<Animator>();

        _originalSpeed = _agent.speed;
        _originalColor = GetComponent<SpriteRenderer>().color;

       // StartCoroutine(RemoveBlockPoint());
    }
    
    private void Update()
    {

        if (!GameManager.Playing) return;
        
        _animator.SetFloat(Movement, _agent.velocity.magnitude);
        _animator.SetBool(IsAttacking, GetComponentInChildren<PlayerShovel>().IsDestructing && !isBlocked);
        
        if (Input.GetMouseButtonDown(0))
        {
            clickSound.Play();

            if (!isBeingCharged && !isBlocked)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
                _agent.destination = mouseWorldPos;   
            }
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

    public void AddChargeCount()
    {
        continuousChargeCount++;
        lightImages[continuousChargeCount - 1].sprite = redLight;
    }

    public IEnumerator BlockCo()
    {
        isBlocked = true;

        blockedSound.Play();
        
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
        StartCoroutine(TurnOffLights());
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
        } 
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Roads"))
        {
            _agent.speed = _originalSpeed;
        } 
    }

    private IEnumerator TurnOffLights()
    {
        for (int i = lightImages.Count - 1; i >= 0; i--)
        {
            lightImages[i].sprite = yellowLight;
            yield return new WaitForSeconds(0.15f);       
        }
    }
}
