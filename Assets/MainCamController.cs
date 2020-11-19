using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamController : MonoBehaviour
{
    private Camera _mainCam;

    public float _pollingSpeed = 0.03f;
    public float _pollingSpeedMultiplier = 5f;

    private Vector3 _currentPos = Vector3.zero;
    private Vector3 _newPos = Vector3.zero;   

    private void Awake()
    {
        _mainCam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        Move();
    }

    private void Zoom()
    {
        if (Input.mouseScrollDelta.y<0)
        {            
            _mainCam.orthographicSize += _pollingSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                 _mainCam.orthographicSize += _pollingSpeed * _pollingSpeedMultiplier;
            }
        }
        else if (Input.mouseScrollDelta.y>0)
        {
            _mainCam.orthographicSize -= _pollingSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _mainCam.orthographicSize -= _pollingSpeed * _pollingSpeedMultiplier;
            }
        }
    }

    private void Move()
    {
        Vector3 _currentPos = _newPos;
        _newPos = Input.mousePosition;
        if (Input.GetMouseButton(2) && _currentPos != Input.mousePosition)
        {            
            Vector3 delta = _mainCam.ScreenToWorldPoint(_currentPos) - _mainCam.ScreenToWorldPoint(_newPos);           
            _mainCam.transform.position += delta;    
            
        }
    }    
}
