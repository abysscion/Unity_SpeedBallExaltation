using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpeningRewardScript : MonoBehaviour
{
    public GameObject box0;
    public GameObject box1;
    public GameObject box2;

    private List<GameObject> boxes;
    private Camera cam;
    private const int RewardMin = 5;
    private const int RewardMax = 15;

    private void Start()
    {
        if (!box0) box0 = GameObject.Find("Box_0");
        if (!box1) box1 = GameObject.Find("Box_1");
        if (!box2) box2 = GameObject.Find("Box_2");
        
        cam = Camera.main;
        boxes = new List<GameObject> {box0, box1, box2};
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
            var touchNear = cam.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));
            var touchFar = cam.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, cam.farClipPlane));

            if (touch.phase == TouchPhase.Ended)
            {
                if (Physics.Raycast(touchNear, touchFar - touchNear, out var hitInfo))
                {
                    foreach (var box in boxes)
                    {
                        if (box == hitInfo.collider.gameObject)
                        {
                            
                        }
                    }
                }
            }
        }
    }
}
