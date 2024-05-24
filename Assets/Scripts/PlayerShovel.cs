using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShovel : MonoBehaviour
{
    
    private bool _isDestructing;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("DestructionZone"))
        {
            if (!_isDestructing) other.gameObject.GetComponentInParent<Building>().StartDestruction();
            _isDestructing = true;
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("DestructionZone"))
        {
            if (_isDestructing)
            {
                var building = other.gameObject.GetComponentInParent<Building>();
                if (building != null)
                {
                    building.StopDestruction();    
                }
                
                _isDestructing = false;
            }
            
        }
    }
}
