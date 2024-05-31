using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    [SerializeField] private GameObject explodingBuildParticlesPrefab;
    [SerializeField] private float destructionTime = 2f;    // TODO: Convertir a segundos
    [SerializeField] private Image fillImage;

    private float _remainingDestruction;
    private IEnumerator _destructionCo;
    private GameObject _explodingBuildParticles;
    
    // SOUNDS
    [SerializeField] private AudioSource fallingBuildingSound;
    [SerializeField] private AudioSource destroyedBuildingSound;
    
    void Start()
    {
        _remainingDestruction = destructionTime;
        _destructionCo = StartDestructionCo();
    }

    public void StartDestruction()
    {
        fallingBuildingSound.Play();
        StartCoroutine(_destructionCo);
    }

    private IEnumerator StartDestructionCo()
    {
        
        FindObjectsByType<Building>(FindObjectsSortMode.None).ToList().ForEach(b =>
        {
            if (!b.gameObject.name.Equals(gameObject.name)) b.StopDestruction();
        });
        
        while (_remainingDestruction > 0)
        {
            if (!PlayerMovement.Instance.isBlocked && !PlayerMovement.Instance.isBeingCharged)
            {
                float lerp = Mathf.PingPong(Time.time, 0.25f);
                GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, lerp);
                
                _remainingDestruction -= Time.deltaTime / destructionTime;
                fillImage.fillAmount = _remainingDestruction / destructionTime;
                
            } else if (GetComponent<SpriteRenderer>().color != Color.white)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            
            yield return null;
        }

        if (_remainingDestruction <= 0.01f)
        {
            destroyedBuildingSound.Play();
            if (CameraShake.Instance.gameObject != null) CameraShake.Instance.ShakeCamera();
            _explodingBuildParticles = Instantiate(explodingBuildParticlesPrefab, transform.position, Quaternion.identity);
            _explodingBuildParticles.GetComponentsInChildren<ParticleSystem>().ToList().ForEach(p => p.Play());
            
            NavMeshManager.Instance.UpdateNavMesh();
            Destroy(gameObject);

            if (FindObjectsByType<Building>(FindObjectsSortMode.None).Length == 1)
            {
                GameManager.Instance.EndGame(true);
            }
        }
    }

    public void StopDestruction()
    {
        StopCoroutine(_destructionCo);
        fallingBuildingSound.Pause();
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    
    
    
}
