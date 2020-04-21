using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    public bool canControlWithMouse;

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
        _cameraTransform.position = new Vector3(2.5f, transform.position.y + 1.8f, -8f);
        // _cameraTransform.position = new Vector3(1.5f, transform.position.y + 0.5f, -4.0f);
        if (canControlWithMouse)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChangeBallState();
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    ChangeBallState();
                }
            }
        }
    }

    private void Jump()
    {
        Vector3 forceVector = new Vector3(0.0f, 10.0f, 0.0f);
        _jumperRb.AddForce(forceVector, ForceMode.VelocityChange);
        _jumperRb.AddTorque(new Vector3(5.0f, 0, 0), ForceMode.Impulse);
    }

    private void ChangeBallState()
    {
        if (_isStay)
        {
            _jumperRb.constraints = RigidbodyConstraints.FreezePositionX |
                                    RigidbodyConstraints.FreezePositionZ;
                                    // | RigidbodyConstraints.FreezeRotation;
            Jump();
        }
        else
        {
            _jumperRb.constraints =
                RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }

        _isStay = !_isStay;
    }
}
