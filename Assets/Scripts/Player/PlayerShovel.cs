using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShovel : MonoBehaviour
{
    
    public bool IsDestructing { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("DestructionZone"))
        {
            if (!IsDestructing && !PlayerMovement.Instance.isEndingGame) other.gameObject.GetComponentInParent<Building>().StartDestruction();
            IsDestructing = true;
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("DestructionZone"))
        {
            if (IsDestructing)
            {
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
