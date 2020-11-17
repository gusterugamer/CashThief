using Blastproof.Systems.Core.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;
    private Camera _mainCam;


    private Vector3 _lastFingerPos = Vector3.zero;
    private Vector3 _currentPos = Vector3.zero;

    private bool _isInMotion = false;

    [SerializeField] private FloatVariable _playerSpeed;

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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
        {
            _rb.velocity = Vector3.zero;
            _isInMotion = false;
        }
    }
}
