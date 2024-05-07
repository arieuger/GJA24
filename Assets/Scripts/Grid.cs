using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid
{

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    private Cell[,] _gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._originPosition = originPosition;

        this._gridArray = new Cell[width, height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                _gridArray[x, y] = new Cell(x, y);

                /*
                _gridArray[x, y].Usable = !GridManager.Instance.IsCollidingWithRoad(GetWorldPosition(x, y));
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), _gridArray[x, y].Usable ? Color.white : Color.red, 100f);    // Print bottom
                Debug.DrawLine(GetWorldPosition(x + 1, y), GetWorldPosition(x, y), _gridArray[x, y].Usable ?  Color.white : Color.red, 100f);    // Print left
                */
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);    // Print bottom
                Debug.DrawLine(GetWorldPosition(x + 1, y), GetWorldPosition(x, y), Color.white, 100f);    // Print left
            }
        }
        
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        
    }

    public void DrawCenterOfCells()
    {
        foreach (Cell cell in _gridArray)
        {
            if (!cell.Usable)
            {
                Gizmos.DrawSphere(GetCenteredWorldPosition(cell.XPosition, cell.YPosition), 0.075f);
            }
        }
    }

    // public void SetValue(int x, int y, int value)
    // {
    //     if (x >= 0 && y >= 0 && x < _width && y < _height) _gridArray[x, y] = value;
    // }

    // public void SetValue(Vector3 worldPosition, int value)
    // {
    //     int x, y;
    //     GetXY(worldPosition, out x, out y);
    //     SetValue(x, y, value);
    // }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    private Vector3 GetCenteredWorldPosition(int x, int y)
    {
        return GetWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * 0.5f;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }
}
