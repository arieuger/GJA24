using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    [SerializeField] private float destructionTime = 2f;    // TODO: Convertir a segundos
    [SerializeField] private Slider slider;

    private float _remainingDestruction;
    private IEnumerator _destructionCo;
    
    void Start()
    {
        _remainingDestruction = destructionTime;
        _destructionCo = StartDestructionCo();
    }
    
    void Update()
    {
        
    }

    public void StartDestruction()
    {
        StartCoroutine(_destructionCo);
    }

    private IEnumerator StartDestructionCo()
    {
        while (_remainingDestruction > 0)
        {
            if (!PlayerMovement.Instance.isBlocked && !PlayerMovement.Instance.isBeingCharged)
            {
                _remainingDestruction -= Time.deltaTime / destructionTime;
                slider.value = _remainingDestruction / destructionTime;   
            }
            yield return null;
        }

        if (_remainingDestruction <= 0.01f)
        {
            Destroy(gameObject);
            NavMeshManager.Instance.UpdateNavMesh();
        }
    }
    
    public void StopDestruction()
    {
        StopCoroutine(_destructionCo);
    }
    
    
    
}
