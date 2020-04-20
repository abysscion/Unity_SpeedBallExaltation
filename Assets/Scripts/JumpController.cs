using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    private bool _isStay;
    private Rigidbody _jumperRb;
    private Transform _cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        _isStay = true;
        _jumperRb = GetComponent<Rigidbody>();
        _cameraTransform = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        _cameraTransform.position = new Vector3(1.5f, transform.position.y + 0.5f, -4.0f);
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (_isStay)
                {
                    _jumperRb.constraints = RigidbodyConstraints.FreezePositionX |
                                            RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                    Jump();
                }
                else
                {
                    _jumperRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                }

                _isStay = !_isStay;
            }
        }
    }

    private void Jump()
    {
        Vector3 forceVector = new Vector3(0.0f, 10.0f, 0.0f);
        _jumperRb.AddForce(forceVector, ForceMode.VelocityChange);
    }
}
