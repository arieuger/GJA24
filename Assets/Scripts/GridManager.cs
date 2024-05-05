using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    private Grid grid;
    [SerializeField] private List<PolygonCollider2D> roadCollliders;
    [SerializeField] private Transform originPosition;
    [SerializeField] private int width;
    [SerializeField] private int height;
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    
    void Start()
    {
        grid = new Grid(width, height, 1f, originPosition.position);
    }

    public bool IsCollidingWithRoad(Vector3 gridPosition)
    {
        foreach (PolygonCollider2D roadCollider in roadCollliders)
        {
            // if (roadCollider.OverlapPoint(gridPosition)) return true;
            Collider2D overlapArea = Physics2D.OverlapArea(gridPosition, gridPosition + new Vector3(1f, 1f));
            if (overlapArea != null && overlapArea.tag.Equals("Roads"))
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        grid.DrawCenterOfCells();
    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("Holi");
        //     grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
        // }
    }
}
