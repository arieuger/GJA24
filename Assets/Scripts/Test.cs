using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private Grid grid;
    [SerializeField] private Transform originPosition;
    [SerializeField] private int width;
    [SerializeField] private int height;
    
    void Start()
    {
        grid = new Grid(width, height, 1f, originPosition.position);
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
