using UnityEngine;
using Debug = UnityEngine.Debug;

public class JumpController : MonoBehaviour
{
    public Vector3 pullStartPosition = new Vector3(0, 1, -2);          //experimental
    public Vector3 pullEndPosition = new Vector3(0, 0.25f, -1.79f);    //experimental
    public GameObject bendingPole;
    public GameObject bendingPoleTarget;
    public float boostCenterMultiplier = 2.0f;
    public float boostMultiplier = 1.5f;
    public bool canDrawDebug;
    public bool isStick;

    private const float MaxSwipeLength = ForceLimit * MagicalForceDivider;
    private const float MagicalForceDivider = 40.0f; //idk how to name it
    private const float MinSwipeLength = 200.0f;
    private const float ForceLimit = 15.0f;
    
    private SkinnedMeshRenderer _bendingPoleRenderer;
    private Transform _cameraTransform;
    private Rigidbody _jumperRb;
    private Animator _bendingPoleAnimator;
    private Vector2 _firstTouchPos;
    private Vector3 _bendingPoleTargetPosOnTouch;
    private Vector3 _ballPositionOnTouch;
    private Vector3 _ballPullingStep;
    private float _jumpMultiplier;
    private bool _ableToControl;

    private void Start()
    {
        bendingPole = bendingPole == null ? GameObject.Find("BendingPole") : bendingPole;
        bendingPoleTarget = bendingPoleTarget == null ? GameObject.Find("PoleTarget") : bendingPoleTarget;
        _ballPullingStep = (pullEndPosition - pullStartPosition) / (MaxSwipeLength - MinSwipeLength);
        _ableToControl = true;
        _bendingPoleAnimator = bendingPole.GetComponentInChildren<Animator>();
        _bendingPoleRenderer = bendingPole.GetComponentInChildren<SkinnedMeshRenderer>();
        _jumpMultiplier = 1.0f;
        _jumperRb = GetComponent<Rigidbody>();
        _cameraTransform = GameObject.Find("Main Camera").transform;
        StickBall();
    }

    private void Update()
    {
        // TODO не двигать камеру вместе с оттягиванием мяча
        if (GameController.currentGameState != GameController.GameState.Lose)
            _cameraTransform.position = new Vector3(2.5f, transform.position.y + 1.8f, -8f);
        if (!_ableToControl)
            return;
        
        // if (Input.touchCount > 0) //TODO: replace 
        // {
        //  var touch = Input.GetTouch(0);
        var touches = InputHelper.GetTouches();
        if (touches.Count > 0)
        {
            var touch = touches[0];
            if (GameController.currentGameState == GameController.GameState.Lose)
                if (touch.phase == TouchPhase.Began)
                    GameController.RestartLevel();
            if (isStick)
                PrepareToJump(touch);
            else
                PrepareToStick(touch);
        }
    }

    private void PrepareToJump(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                SomeStuffWhenBallSticked(touch.position.x, touch.position.y);
                break;
            case TouchPhase.Moved:
                MoveBallToJumpHigher(touch);
                break;
            case TouchPhase.Ended:
            {
                var secondTouchPos = new Vector2(touch.position.x, touch.position.y);
                var force = CalculateJumpForce(_firstTouchPos.y - secondTouchPos.y);
                
                if (force <= 0.0f) 
                    return;
                if (GameController.currentGameState == GameController.GameState.StartGame)
                    GameController.currentGameState = GameController.GameState.InGame;
                UnstickBall();
                Jump(force);
                _jumperRb.transform.position = new Vector3(0.0f, _jumperRb.transform.position.y, _ballPositionOnTouch.z);
                break;
            }
        }
    }

    private void PrepareToStick(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            var hitTag = this.GetHitTag();
            switch (hitTag)
            {
                case null:
                    StickBall();
                    SomeStuffWhenBallSticked(touch.position.x, touch.position.y);
                    break;
                case "MetallicBarrier":
                    HitMetallicBarrier();
                    break;
                case "RedBarrier":
                    HitRedBarrier();
                    break;
                case "JumpBooster":
                    HitJumpBooster();
                    SomeStuffWhenBallSticked(touch.position.x, touch.position.y);
                    break;
                case "JumpBoosterCenter":
                    HitJumpBoosterCenter();
                    SomeStuffWhenBallSticked(touch.position.x, touch.position.y);
                    break;
            }
        }
    }

    private void HitRedBarrier()
    {
        StickBall();
        GameController.currentGameState = GameController.GameState.Lose;
        UnstickBall();
    }

    private void HitMetallicBarrier()
    {
        StickBall();
        UnstickBall();
    }

    private void HitJumpBooster()
    {
        StickBall();
        _jumpMultiplier = boostMultiplier;
    }
    
    private void HitJumpBoosterCenter()
    {
        StickBall();
        _jumpMultiplier = boostCenterMultiplier;
    }

    private void Jump(float force)
    {
        var forceVector = new Vector3(0.0f, force, 0.0f);
        
        _jumperRb.isKinematic = false;
        _jumperRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        _jumperRb.AddForce(forceVector, ForceMode.VelocityChange);
        _jumperRb.AddTorque(new Vector3(5.0f, 0, 0), ForceMode.Impulse);
        _jumpMultiplier = 1.0f;
    }

    private void StickBall()
    {
        isStick = true;
        _jumperRb.isKinematic = true;
        _jumperRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        bendingPole.transform.position = new Vector3(0, this.transform.position.y, -0.4f);
        _bendingPoleRenderer.enabled = true;
        _bendingPoleAnimator.Play("StickBendingPole");
        bendingPoleTarget.transform.SetParent(_jumperRb.transform);
    }

    public void UnstickBall()
    {
        isStick = false;
        _jumperRb.isKinematic = false;
        _jumperRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        RestoreBendingPole();
    }

    private void RestoreBendingPole()
    {
        _bendingPoleRenderer.enabled = false; //TODO: change it to animation or smth like that
        bendingPoleTarget.transform.position = _bendingPoleTargetPosOnTouch;
        bendingPoleTarget.transform.SetParent(bendingPole.transform);
    }

    private string GetHitTag()
    {
        //TODO: rethink this someday
        if (canDrawDebug)
            Debug.DrawRay(transform.position, Vector3.forward * 2, Color.green, 2.0f);
        if (!Physics.Raycast(transform.position, Vector3.forward, out var hitRes))
            return null;
        if (hitRes.transform.CompareTag("MetallicBarrier"))
            return "MetallicBarrier";
        if (hitRes.transform.CompareTag("RedBarrier"))
            return "RedBarrier";
        if (hitRes.transform.CompareTag("JumpBooster"))
            return "JumpBooster";
        if (hitRes.transform.CompareTag("JumpBoosterCenter"))
            return "JumpBoosterCenter";
        
        return null;
    }
    
    private void MoveBallToJumpHigher(Touch touch)
    {
        var swipeLength = _firstTouchPos.y - touch.position.y;

        if (swipeLength < MinSwipeLength)
            return;
        if (swipeLength < MaxSwipeLength)
        {
            swipeLength -= MinSwipeLength;
            _jumperRb.position = _ballPositionOnTouch + _ballPullingStep * swipeLength;
        }
    }

    private float CalculateJumpForce(float swipeLength)
    {
        float force;

        if (swipeLength < MinSwipeLength)
            return 0.0f;
        force = swipeLength / MagicalForceDivider;
        force = force >= ForceLimit ? ForceLimit : force;
        return force * _jumpMultiplier;
    }

    private void SomeStuffWhenBallSticked(float touchPosX, float touchPosY)
    {
        _firstTouchPos = new Vector2(touchPosX, touchPosY);
        _ballPositionOnTouch = _jumperRb.position;
        _bendingPoleTargetPosOnTouch = bendingPoleTarget.transform.position;
    }
}
