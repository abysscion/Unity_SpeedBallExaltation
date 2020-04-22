using UnityEngine;

public class JumpController : MonoBehaviour
{
    public bool canDrawDebug;
    public bool canControlWithMouse;
    public float mouseForce = 5.0f;

    private bool _isStick;
    private bool _ableToControl;
    private bool _isAlreadyTouched;
    private Rigidbody _jumperRb;
    private Transform _cameraTransform;
    private GameObject _ballStick;
    private Animator _ballStickAnimator;
    private Vector2 _firstTouchPos;

    private void Start()
    {
        _ableToControl = true;
        _ballStick = GameObject.Find("BallStick");
        _ballStickAnimator = _ballStick.GetComponent<Animator>();
        _isStick = true;
        _jumperRb = GetComponent<Rigidbody>();
        _cameraTransform = GameObject.Find("Main Camera").transform;
        _ballStickAnimator.Play("IdleStick");
    }

    private void Update()
    {
        _cameraTransform.position = new Vector3(2.5f, transform.position.y + 1.8f, -8f);
        // _cameraTransform.position = new Vector3(1.5f, transform.position.y + 0.5f, -4.0f);
        if (!_ableToControl)
            return;
        if (canControlWithMouse)
            HandleMouseControl();
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (_isAlreadyTouched)
            {
                if (touch.phase == TouchPhase.Ended)
                    _isAlreadyTouched = false;
                return;
            }

            if (_isStick)
                PrepareToJump(touch);
            else
                PrepareToStick(touch);
        }
    }

    private void HandleMouseControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isStick)
            {
                _jumperRb.constraints = RigidbodyConstraints.FreezePositionX |
                                        RigidbodyConstraints.FreezePositionZ;
                Jump(mouseForce);
                _ballStickAnimator.Play("RetractStick");
                _isStick = !_isStick;
            }
            else
            {
                var hitTag = GetHitTag();
                switch (hitTag)
                {
                    case null:
                        StickBall();
                        _isStick = !_isStick;
                        _ballStickAnimator.Play("IdleStick");
                        break;
                    case "MetallicBarrier":
                        StickBall();
                        _ballStickAnimator.Play("RetractStick");
                        break;
                    case "RedBarrier":
                        GameController.RestartLevel();
                        break;
                }
            }
        }
    }

    private void Jump(float force)
    {
        Vector3 forceVector = new Vector3(0.0f, force, 0.0f);
        _jumperRb.AddForce(forceVector, ForceMode.VelocityChange);
        _jumperRb.AddTorque(new Vector3(5.0f, 0, 0), ForceMode.Impulse);
    }

    private string GetHitTag()
    {
        if (canDrawDebug)
            Debug.DrawRay(this.transform.position, Vector3.forward * 2, Color.green, 2.0f);
        if (!Physics.Raycast(this.transform.position, Vector3.forward, out var hitRes))
            return null;
        if (hitRes.transform.CompareTag("MetallicBarrier"))
            return "MetallicBarrier";
        if (hitRes.transform.CompareTag("RedBarrier"))
            return "RedBarrier";

        return null;
    }

    private void StickBall()
    {
        _jumperRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        var newPos = this.transform.position;
        newPos += Vector3.forward;
        _ballStick.transform.position = newPos;
    }

    private void MoveBallToJumpHigher()
    {

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
            // _ballStickAnimator.Play("RetractStick");
            _ballStickAnimator.Play("IdleFly");
            _isStick = !_isStick;
        }
    }

    private void PrepareToStick(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            var hitTag = GetHitTag();
            switch (hitTag)
            {
                case null:
                    StickBall();
                    _isStick = !_isStick;
                    _ballStickAnimator.Play("IdleStick");
                    break;
                case "MetallicBarrier":
                    StickBall();
                    _ballStickAnimator.Play("RetractStick");
                    break;
                case "RedBarrier":
                    GameController.RestartLevel();
                    break;

            }
        }
    }
    
    public void ToggleControl()
    {
        _ableToControl = !_ableToControl;
    }
    
    public void FreeBallFromBallStick()
    {
        _jumperRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }
}
