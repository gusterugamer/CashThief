using Blastproof.Systems.Core.Variables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private FloatVariable _playerSpeed;
    [SerializeField] private FloatVariable _forceMagnitude;

    private Rigidbody _rb;

    private Camera _mainCam;

    private Vector3 _lastFingerPos = Vector3.zero;
    private Vector3 _currentPos = Vector3.zero;

    private bool _isInMotion = false;
    private bool _hit = false;    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
    }

    private void Update()
    {       
        Swipe();        
    }

    private void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastFingerPos = Input.mousePosition;            
        }
        else if(Input.GetMouseButtonUp(0) && !_isInMotion)
        {
            _currentPos = Input.mousePosition;

            Move();           
        }
        CheckCollision();
    }

    private void CheckCollision()
    {
        if (Physics.Raycast(transform.position, transform.forward,0.5f) && !_hit)
        {
            StateReset();
             _hit = true;
            Debug.Log("YEP!");
        }
    }

    private void Move()
    {
        float diffX = _currentPos.x - _lastFingerPos.x;
        float diffY = _currentPos.y - _lastFingerPos.y;

        float SignDiffX = Mathf.Sign(diffX);
        float SignDiffY = Mathf.Sign(diffY);

        diffX = Mathf.Abs(diffX);
        diffY = Mathf.Abs(diffY);

        if (diffY > diffX)
        {
            Vector3 newDir = SignDiffY * Vector3.right * _playerSpeed;

            _rb.velocity = newDir;
            transform.forward = newDir;
        }
        else
        {
            Vector3 newDir = SignDiffX * Vector3.back * _playerSpeed;

            _rb.velocity = newDir;
            transform.forward = newDir;
        }

        _isInMotion = true;
        _hit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Box")
        {
            StateReset();

            Vector3 force = transform.forward.normalized * _forceMagnitude;

            collision.transform.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }

    public void Stop()
    {
        _rb.velocity = Vector3.zero;       
    }

    public void StateReset()
    {
        _rb.velocity = Vector3.zero;
        _isInMotion = false;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.tag == "Wall")
    //    {
    //        _rb.velocity = Vector3.zero;
    //        _isInMotion = false;
    //    }
    //}    
}
