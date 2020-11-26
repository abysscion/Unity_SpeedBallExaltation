﻿using System.Collections;
using UnityEngine;
using Utilities;
using Debug = UnityEngine.Debug;

namespace Controllers
{
    public class JumpController : MonoBehaviour
    {
        public GameObject bendingPoleTarget;
        public GameObject bendingPole;
        public bool canDrawDebug;
        public bool isStick;

        private SkinnedMeshRenderer _bendingPoleRenderer;
        private Transform _cameraTransform;
        private Transform _jumperTransform;
        private Rigidbody _jumperRb;
        private Animator _bendingPoleAnimator;
        private Vector3 _ballPositionOnTouch;
        private Vector3 _ballPullingStep;
        private Vector2 _firstTouchPos;
        private readonly Vector3 _defaultBendingPoleTargetPosition = new Vector3(0, 0, -2.0f);
        private readonly Vector3 _pullStartPosition = new Vector3(0, 1, -2);
        private readonly Vector3 _pullEndPosition = new Vector3(0, 0.3f, -1.65f);
        private const float BoostCenterMultiplier = 1.8f;
        private const float MagicalForceDivider = 20.0f;
        private const float ControlLockDelay = 0.2f;
        private const float BoostMultiplier = 1.4f;
        private const float MinSwipeLength = 50.0f;
        private const float ForceLimit = 14.6f;
        private const int MetallicBarrierPause = 6; //frames count
        private float _maxSwipeLength;
        private float _jumpMultiplier;
        private bool _ableToControl;
        private int _metallicTimer;

        public void UnstickBall()
        {
            StartCoroutine(nameof(UnlockControl));
            _ableToControl = false;
            _jumperRb.isKinematic = false;
            _jumperRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            isStick = false;
            _jumperTransform.position = new Vector3(0.0f, _jumperTransform.position.y, _ballPositionOnTouch.z);
            RestoreBendingPole();
        }
    
        private void Start()
        {
            _maxSwipeLength = ForceLimit * MagicalForceDivider;
            bendingPole = bendingPole == null ? GameObject.Find("BendingPole") : bendingPole;
            bendingPoleTarget = bendingPoleTarget == null ? GameObject.Find("PoleTarget") : bendingPoleTarget;
            _ballPullingStep = (_pullEndPosition - _pullStartPosition) / (_maxSwipeLength - MinSwipeLength);
            _ableToControl = true;
            _bendingPoleAnimator = bendingPole.GetComponentInChildren<Animator>();
            _bendingPoleRenderer = bendingPole.GetComponentInChildren<SkinnedMeshRenderer>();
            _jumpMultiplier = 1.0f;
            _jumperTransform = this.transform;
            _jumperRb = this.GetComponent<Rigidbody>();
            _cameraTransform = GameObject.Find("Main Camera").transform;
            _cameraTransform.position = new Vector3(2.5f, transform.position.y + 1.8f, -8f);
            StickBall();
        }

        private void Update()
        {
            if (!isStick && GameController.CurrentGameState != GameController.GameState.Lose)
            {
                if (_jumperRb.velocity.y > 0.0f)
                {
                    if (_jumperTransform.position.y + 1.8f >= _cameraTransform.position.y)
                        _cameraTransform.position = new Vector3(2.5f, transform.position.y + 1.8f, -8f);
                }
                else
                    _cameraTransform.position = new Vector3(2.5f, transform.position.y + 1.8f, -8f);
            }
            if (!_ableToControl || GameController.CurrentGameState == GameController.GameState.ChooseBall ||
                GameController.CurrentGameState == GameController.GameState.Menu)
                return;
            if (_metallicTimer != 0)
            {
                _metallicTimer--;
                if (_metallicTimer == 0)
                    UnstickBall();
                return;
            }
            var touches = InputHelper.GetTouches(); //TODO: when debug
            if (touches.Count > 0)
            {
                var touch = touches[0];
            // if (Input.touchCount > 0)
            // {
            //  var touch = Input.GetTouch(0);
                if (GameController.CurrentGameState == GameController.GameState.Lose)
                {
                    if (touch.phase == TouchPhase.Began) 
                        GameController.Instance.RestartLevel();
                }
                if (isStick)
                    PrepareToJump(touch);
                else
                    PrepareToStick(touch);
            }
        }

        private void PrepareToJump(Touch touch)
        {
            if (GameController.CurrentGameState == GameController.GameState.Lose) 
                return;
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    SomeStuffWhenBallStuck(touch.position.x, touch.position.y);
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
                    if (GameController.CurrentGameState == GameController.GameState.StartGame)
                        GameController.CurrentGameState = GameController.GameState.InGame;
                    UnstickBall();
                    Jump(force);
                    SoundController.Instance.PlaySound(SoundController.SoundName.ReleaseBendingPole);
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
                        SomeStuffWhenBallStuck(touch.position.x, touch.position.y);
                        SoundController.Instance.PlaySound(SoundController.SoundName.StickBendingPole);
                        break;
                    case "MetallicBarrier":
                        HitMetallicBarrier();
                        SomeStuffWhenBallStuck(touch.position.x, touch.position.y);
                        SoundController.Instance.PlaySound(SoundController.SoundName.MetallicAsteroidHit);
                        break;
                    case "RedBarrier":
                        HitRedBarrier();
                        break;
                    case "JumpBooster":
                        SomeStuffWhenBallStuck(touch.position.x, touch.position.y);
                        HitJumpBooster();
                        break;
                    // case "JumpBoosterCenter":
                    //     HitJumpBoosterCenter();
                    //     SomeStuffWhenBallStuck(touch.position.x, touch.position.y);
                    //     break;
                }
            }
        }

        private void HitRedBarrier()
        {
            StickBall();
            GameController.CurrentGameState = GameController.GameState.Lose;
            UnstickBall();
        }

        private void HitMetallicBarrier()
        {
            StickBall();
            _metallicTimer = MetallicBarrierPause;
        }

        private void HitJumpBooster()
        {
            Physics.Raycast(transform.position, Vector3.forward, out var hitRes);
            if (hitRes.transform.position.y < transform.position.y + 0.2f &&
                hitRes.transform.position.y > transform.position.y - 0.2f)
            {
                _jumpMultiplier = BoostCenterMultiplier;
            }
            else
            {
                _jumpMultiplier = BoostMultiplier;    
            }
            StickBall();
        }
    
        // private void HitJumpBoosterCenter()
        // {
        //     StickBall();
        //     _jumpMultiplier = BoostCenterMultiplier;
        // }

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

        private IEnumerator UnlockControl()
        {
            yield return new WaitForSecondsRealtime(ControlLockDelay);
        
            _ableToControl = true;
        }

        private void RestoreBendingPole()
        {
            _bendingPoleRenderer.enabled = false;
            bendingPoleTarget.transform.SetParent(bendingPole.transform);
            bendingPoleTarget.transform.localPosition = _defaultBendingPoleTargetPosition;
        }

        private string GetHitTag()
        {
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
            {
                _jumperRb.position = _ballPositionOnTouch;
                return;
            }
            if (swipeLength <= _maxSwipeLength)
            {
                swipeLength -= MinSwipeLength;
                _jumperRb.position = _ballPositionOnTouch + _ballPullingStep * swipeLength;
            }

            if (swipeLength > _maxSwipeLength)
            {
                _jumperRb.position = _ballPositionOnTouch + _ballPullingStep * (_maxSwipeLength - MinSwipeLength);
            }
        }

        private float CalculateJumpForce(float swipeLength)
        {
            if (swipeLength < MinSwipeLength)
                return 0.0f;
            
            var force = swipeLength / MagicalForceDivider;
            force = force >= ForceLimit ? ForceLimit : force;
            return force * _jumpMultiplier;
        }

        private void SomeStuffWhenBallStuck(float touchPosX, float touchPosY)
        {
            _firstTouchPos = new Vector2(touchPosX, touchPosY);
            _ballPositionOnTouch = _jumperRb.position;
        }
    }
}
