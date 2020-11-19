using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditor : SerializedMonoBehaviour
{
    [SerializeField] private LevelCreator lc;

    [TableMatrix(SquareCells = true)]
    public Mesh[,] CustomCellDrawing = new Mesh[LevelCreator.MATRIX_SIZE, LevelCreator.MATRIX_SIZE];

    private static bool DrawCell(Rect rect, bool value)
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
    private void Read()
    {
        int size = LevelCreator.MATRIX_SIZE;

        var arr = lc.CustomCellDrawing;

        for (int i=0;i<size;i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (arr[i,j])
                {
                    Mesh mesh = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>().mesh;
                    CustomCellDrawing[i, j] = mesh;
                }
            }
        }
    }
}
