using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelCreator : SerializedMonoBehaviour
{  
    public const int MATRIX_SIZE = 20;

    private int _rowSize = 0;

    private LevelProperties lp;

    [TableMatrix(DrawElementMethod = "DrawCell", SquareCells = true)]
    public bool[,] CustomCellDrawing = new bool[MATRIX_SIZE, MATRIX_SIZE];

    private static bool DrawCell (Rect rect, bool value)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }

        EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.5f, 0.2f) : new Color(0f, 0f, 0.5f));

        return value;
    }

    [Button]
    private void Print()
    {
        int size = CustomCellDrawing.Length;

        _rowSize = (int)Math.Sqrt(size);

        for (int i=0;i< _rowSize; i++)
        {
            for (int j = 0; j < _rowSize; j++)
            {
                Debug.Log("i: " + i + " j: " + j + "state: " + CustomCellDrawing[i,j]);
            }
        }
    }

    [Button]
    private void GenerateLevel()
    {
        lp = new LevelProperties();       

        int size = CustomCellDrawing.Length;

        _rowSize = (int)Math.Sqrt(size);

        GameObject center = new GameObject();
        center.name = "Center";      

        center.transform.position = Vector3.zero;

        Vector3 startInstancePosition = center.transform.position + new Vector3(center.transform.localScale.x/2, 0f, center.transform.localScale.z/2);

        for (int i = 0; i < _rowSize; i++)
        {
            for (int j = 0; j < _rowSize; j++)
            {
                if (CustomCellDrawing[i,j])
                {
                    GameObject instance = GameObject.CreatePrimitive(PrimitiveType.Cube);                                  
                    instance.transform.position = startInstancePosition + new Vector3(i*instance.transform.localScale.x, 0.0f,j*instance.transform.localScale.z);
                    instance.transform.parent = center.transform;
                    instance.GetComponent<MeshRenderer>().material.color = Color.blue;
                    lp.pos.Add(instance.transform.position);
                }
            }
        }
    }

    [Button]
    private void SaveLevel()
    {
        var json = JsonUtility.ToJson(lp, true);
        File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "newGeneratedLevel"), json);
    }
}
