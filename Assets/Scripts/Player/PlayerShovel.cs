using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShovel : MonoBehaviour
{
    
    [SerializeField] private GameObject smokeParticlesPrefab;
    public bool IsDestructing { get; private set; }
    private ParticleSystem _smokeParticles;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("DestructionZone"))
        {
            if (!IsDestructing && !PlayerMovement.Instance.isEndingGame)
            {
                other.gameObject.GetComponentInParent<Building>().StartDestruction();
                _smokeParticles = Instantiate(smokeParticlesPrefab, other.ClosestPoint(transform.position), Quaternion.identity).GetComponent<ParticleSystem>();
                _smokeParticles.Play();
            }
            IsDestructing = true;
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("DestructionZone"))
        {
            if (IsDestructing)
            {
                if (_smokeParticles) _smokeParticles.Stop();
                var building = other.gameObject.GetComponentInParent<Building>();
                if (building != null)
                {
                    building.StopDestruction();    
                }
                
                IsDestructing = false;
            }
            
        }
    }
}
