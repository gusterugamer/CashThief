using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SEEMINMAX" , menuName = "SEEMINMAX")]
public class SeeMinMax : ScriptableObject
{
    [SerializeField] private GameObject _target1;
    [SerializeField] private GameObject _target2;

    public float distance;

    private Vector3 _posT1;
    private Vector3 _posT2;

    [Button]
    private void ShowMax()
    {
        GameObject maxT1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject maxT2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        maxT1.transform.parent = _target1.transform;
        maxT2.transform.parent = _target2.transform;

        Mesh meshT1 = _target1.GetComponent<MeshFilter>().mesh;
        Mesh meshT2 = _target2.GetComponent<MeshFilter>().mesh;

        _posT1 = maxT1.transform.position = meshT1.bounds.max;
        _posT2 = maxT2.transform.position = meshT2.bounds.max;

      
    }

    [Button]
    private void ShowMin()
    {
        GameObject minT1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject minT2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        minT1.transform.parent = _target1.transform;
        minT2.transform.parent = _target2.transform;

        Mesh meshT1 = _target1.GetComponent<MeshFilter>().mesh;
        Mesh meshT2 = _target2.GetComponent<MeshFilter>().mesh;

        _posT1 = minT1.transform.position = meshT1.bounds.min;
        _posT2 = minT2.transform.position = meshT2.bounds.min;
    }

    [Button]
    private void CalculateDistanceWithBoxCollider()
    {
        BoxCollider _bT1 = _target1.GetComponent<BoxCollider>();
        BoxCollider _bT2 = _target2.GetComponent<BoxCollider>();       

        Vector3 bPosT1 = _bT1.bounds.max; ;
     //   bPosT1 = _bT1.transform.TransformPoint(bPosT1);     

        Vector3 bPosT2 = new Vector3 (_bT2.bounds.max.x, _bT2.bounds.max.y, _bT2.bounds.min.z);

       // bPosT2 = _bT2.transform.TransformPoint(bPosT2);

        GameObject minT1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject minT2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        minT1.transform.localScale = Vector3.one * 0.1f;
        minT2.transform.localScale = Vector3.one * 0.1f;

        minT1.transform.position = bPosT1;
        minT2.transform.position = bPosT2;

        distance = Vector3.Distance(bPosT1, bPosT2);
    }


}
