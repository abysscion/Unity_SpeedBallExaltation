using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpController : MonoBehaviour
{
    public bool canDrawDebug;
    public bool canControlWithMouse;

    private bool _isStay;
    private Rigidbody _jumperRb;
    private Transform _cameraTransform;
    private GameObject _ballStick;
    
    private void Start()
    {
        _ballStick = GameObject.Find("BallStick");
        _ballStick.transform.Translate(0,0,1.0f);
        _ballStick.SetActive(false);
        _isStay = true;
        _jumperRb = GetComponent<Rigidbody>();
        _cameraTransform = GameObject.Find("Main Camera").transform;
    }
    
    private void Update()
    {
        _cameraTransform.position = new Vector3(2.5f, transform.position.y + 1.8f, -8f);
        // _cameraTransform.position = new Vector3(1.5f, transform.position.y + 0.5f, -4.0f);
        if (canControlWithMouse)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CheckIfAbleToStick())
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
                    if (CheckIfAbleToStick())
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
            _jumperRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            _ballStick.SetActive(false);
            Jump();
        }
        else
        {
            _jumperRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            var newPos = this.transform.position;
            newPos += Vector3.forward;
            _ballStick.transform.position = newPos;
            _ballStick.SetActive(true);
        }

        _isStay = !_isStay;
    }

    private bool CheckIfAbleToStick()
    {
        if (canDrawDebug)
            Debug.DrawRay(this.transform.position, Vector3.forward * 2, Color.green, 2.0f);

        if (Physics.Raycast(this.transform.position, Vector3.forward, out var hitRes))
        {
            if (hitRes.transform.CompareTag("MetallicBarrier"))
                return false;
            if (hitRes.transform.CompareTag("RedBarrier"))
            {
                SceneManager.LoadScene("SampleScene");
                return false;
            }
        }
        return true;
    }
}
