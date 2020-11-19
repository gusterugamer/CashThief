using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : SerializedMonoBehaviour
{
    private int _width = 0;   
    private int _height = 0;

    private Camera _mainCam;

    //TEMPSHIT
    [SerializeField] GameObject prefab;
    [SerializeField] private int _cellSize;
    //TEMPSHIT

    TileData[,] tilesdata;

    private void Start()
    {      
    
    }

    [Button]
    private void ChangeGridDimensions()
    {
        _mainCam = Camera.main;

        RectTransform rt = GetComponent<RectTransform>();        

        float canvasW = rt.sizeDelta.x;
        float canvasH = rt.sizeDelta.y;

        Vector2 canvasCenter = new Vector3(canvasW / 2, canvasH / 2);

        rt.position = canvasCenter;

        if (canvasW % 10 != 0 || canvasH % 10 != 0)
        {
            //Debug.LogError("Use integers when define the grid dimensions!");

            //TODO: Fix for any number
        }

        _width = (int)rt.sizeDelta.x;
        _height = (int)rt.sizeDelta.y;

        float maxDim = Mathf.Max(_width, _height);

        _mainCam.orthographicSize = 0.6f * maxDim;
        _mainCam.transform.position = new Vector3(canvasW / 2, canvasH / 2, -1f);

        tilesdata = new TileData[_width,_height];

        SetTile(new Vector2(43.2f, 50.6767f));
    }

    private void OnDrawGizmos()
    {
        for (int i = 1; i < _width; i++)
        {
            Gizmos.DrawLine(new Vector3(i-0.5f, 0.0f, 0.0f), new Vector3(i-0.5f, _height, 0.0f));
        }

        for (int i = 1; i < _height; i++)
        {
            Gizmos.DrawLine(new Vector3(0, i-0.5f, 0.0f), new Vector3(_width, i-0.5f, 0.0f));
        }
    }

    private void DrawGrid()
    {
      
    }

    public void SetTile(Vector2 position)
    {
        PosToIndex(position);
    }

    private void PosToIndex(Vector2 position)
    {
        if (isInGrid(position))
        {
            int indexRow = (int)position.x;
            int indexColoumn = (int)position.y;

            TileData newTile = new TileData();
            newTile.position = new Vector2Int(indexRow,indexColoumn);

            GameObject pref = Instantiate(prefab);
            pref.transform.position = new Vector2(indexRow, indexColoumn);

            tilesdata[indexRow, indexColoumn] = newTile;
        }
    }

    private bool isInGrid(Vector2 position)
    {
        return (position.x >= 0.0f && position.y >= 0.0f) &&
               (position.x <= _width && position.y <= _height);
    }

}
