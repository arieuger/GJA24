using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    [SerializeField] private float destructionTime = 2f;    // TODO: Convertir a segundos
    [SerializeField] private Image fillImage;
    [SerializeField] private Color notBeingDestructedColor;
    [SerializeField] private Color beingDestructedColor;
    [SerializeField] private SpriteRenderer destructionZoneSquare;

    private float _remainingDestruction;
    private IEnumerator _destructionCo;
    
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
        while (_remainingDestruction > 0)
        {
            if (!PlayerMovement.Instance.isBlocked && !PlayerMovement.Instance.isBeingCharged)
            {
                if (destructionZoneSquare.color != beingDestructedColor) destructionZoneSquare.color = beingDestructedColor;
                
                _remainingDestruction -= Time.deltaTime / destructionTime;
                fillImage.fillAmount = _remainingDestruction / destructionTime;
                
            } else if (destructionZoneSquare.color != notBeingDestructedColor)
            {
                destructionZoneSquare.color = notBeingDestructedColor;
            }
            
            yield return null;
        }

        if (_remainingDestruction <= 0.01f)
        {
            destroyedBuildingSound.Play();
            Destroy(gameObject);
            NavMeshManager.Instance.UpdateNavMesh();
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
        destructionZoneSquare.color = notBeingDestructedColor;
    }
    
    
    
}
