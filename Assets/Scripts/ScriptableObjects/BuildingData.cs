using UnityEngine;

[CreateAssetMenu(fileName = "New Area Data")]
public class BuildingData : ScriptableObject
{

    [SerializeField] private float xSize;
    [SerializeField] private float ySize;
    
    public float XSize => xSize;
    public float YSize => ySize;
    
}
