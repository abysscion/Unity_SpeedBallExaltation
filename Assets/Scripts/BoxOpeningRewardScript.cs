using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class BoxOpeningRewardScript : MonoBehaviour
{
    public GameObject box0;
    public GameObject box1;
    public GameObject box2;

    private List<GameObject> _boxes;
    private Camera _cam;
    private const int RewardMin = 5;
    private const int RewardMax = 15;

    private void Start()
    {
        if (!box0) box0 = GameObject.Find("Box_0");
        if (!box1) box1 = GameObject.Find("Box_1");
        if (!box2) box2 = GameObject.Find("Box_2");
        
        _cam = Camera.main;
        _boxes = new List<GameObject> {box0, box1, box2};
    }
    
    private void Update()
    {
        // if (Input.touchCount > 0) //TODO: replace on release
        // {
        //  var touch = Input.GetTouch(0);
        var touches = InputHelper.GetTouches();
        if (touches.Count > 0)
        {
            var touch = touches[0];
            var touchNear = _cam.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, _cam.nearClipPlane));
            var touchFar = _cam.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, _cam.farClipPlane));

            if (touch.phase == TouchPhase.Ended)
            {
                if (Physics.Raycast(touchNear, touchFar - touchNear, out var hitInfo))
                {
                    foreach (var box in _boxes)
                    {
                        if (box == hitInfo.collider.gameObject)
                        {
                            var newCamPos = box.transform.position;

                            newCamPos.z = -4;
                            box.GetComponent<Rotator>().enabled = false;
                            this.GetComponent<SmoothCameraTranslator>().MoveTowards(newCamPos);
                            this.GetComponent<SmoothObjectRotationResetor>().RestoreBoxRotation(box);
                            StartCoroutine(nameof(RunGiftReceivingSequence));
                        }
                    }
                }
            }
        }
    }
    
    private IEnumerator RunGiftReceivingSequence()
    {
        yield return new WaitForSecondsRealtime(this.GetComponent<SmoothObjectRotationResetor>().timeToRestore);
        
        //play rotate-n-poof anim?
        //coins anim?
        //go next button
        // vvvvvvvvvvv
        //load_scene(segmented_scene) || game controller state = level start
    }
}

//TODO: SkinController.cs:45 fix update method (switch-case)
