using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    [SerializeField] private float destructionTime = 2f;    // TODO: Convertir a segundos
    [SerializeField] private Slider slider;

    private float _remainingDestruction;
    
    void Start()
    {
        _remainingDestruction = destructionTime;
    }
    
    void Update()
    {
        
    }

    public void StartDestruction()
    {
        StartCoroutine(StartDestructionCo());
    }

    private IEnumerator StartDestructionCo()
    {
        while (_remainingDestruction > 0)
        {
            _remainingDestruction -= Time.deltaTime / destructionTime;
            Debug.Log(_remainingDestruction);
            slider.value = _remainingDestruction / destructionTime;
            yield return null;
        }

        if (_remainingDestruction <= 0.01f)
        {
            Destroy(gameObject);
            // TODO: Reload NavMesh
        }
    }
    
    public void StopDestruction()
    {
        
    }
    
    
    
}
