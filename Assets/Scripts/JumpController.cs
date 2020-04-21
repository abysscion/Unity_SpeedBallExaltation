using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpController : MonoBehaviour
{
 public bool canDrawDebug;
    public bool canControlWithMouse;

    private bool _isStay;
    private bool _isAlreadyTouched;
    private Rigidbody _jumperRb;
    private Transform _cameraTransform;
    private Vector2 _firstTouchPos;

    void Start()
    {
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
                if (_isAlreadyTouched)
                {
                    if (touch.phase == TouchPhase.Ended)
                    {
                        _isAlreadyTouched = false;
                    }
                    return;
                }
                if (_isStay)
                {
                    PrepareToJump(touch);
                }
                else
                {
                    PrepareToStick(touch);
                }
            }
        }
    }

    private void PrepareToJump(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            _firstTouchPos = new Vector2(touch.position.x, touch.position.y);
        }

        if (touch.phase == TouchPhase.Moved)
        {
            MoveBallToJumpHigher();
        }
        if (touch.phase == TouchPhase.Ended)
        {
            Vector2 secondTouchPos = new Vector2(touch.position.x, touch.position.y);
            float force = _firstTouchPos.y - secondTouchPos.y;
            if (force < 200)
            {
                return;
            }
            _jumperRb.constraints = RigidbodyConstraints.FreezePositionX |
                                    RigidbodyConstraints.FreezePositionZ;
            force /= 40.0f;
            force = force >= 15.0f ? 15.0f : force;
            Jump(force);
            _isStay = !_isStay;
        }
    }

    private void PrepareToStick(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            if (CheckIfAbleToStick())
            {
                _jumperRb.constraints = RigidbodyConstraints.FreezePosition |
                                        RigidbodyConstraints.FreezeRotation;
                _isStay = !_isStay;
            }
            _isAlreadyTouched = true;
        }
    }
    
    private void ChangeBallState()
    {
        if (_isStay)
        {
            _jumperRb.constraints = RigidbodyConstraints.FreezePositionX |
                                    RigidbodyConstraints.FreezePositionZ;
            Jump(10.0f);
        }
        else
        {
            _jumperRb.constraints = RigidbodyConstraints.FreezePosition |
                                    RigidbodyConstraints.FreezeRotation;
        }
        _isStay = !_isStay;
    }

    private void Jump(float force)
    {
        Vector3 forceVector = new Vector3(0.0f, force, 0.0f);
        _jumperRb.AddForce(forceVector, ForceMode.VelocityChange);
        _jumperRb.AddTorque(new Vector3(5.0f, 0, 0), ForceMode.Impulse);
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
                GameController.RestartLevel();
                return false;
            }
        }
        return true;
    }

    private void MoveBallToJumpHigher()
    {
        
    }
}
