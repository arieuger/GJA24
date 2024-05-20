using System;
using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{

    private NavMeshSurface _surface;
    
    public static NavMeshManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        _surface = GetComponent<NavMeshSurface>();
    }

    public void UpdateNavMesh()
    {
       _surface.UpdateNavMesh(_surface.navMeshData);
    }
    
}
