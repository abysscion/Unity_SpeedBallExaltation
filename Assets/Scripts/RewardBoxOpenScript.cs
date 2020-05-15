using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class RewardBoxOpenScript : MonoBehaviour
{
    public GameObject box0;
    public GameObject box1;
    public GameObject box2;

    private List<GameObject> _boxes;
    private Text _coinsAmountText;
    private Camera _cam;
    private const int RewardMin = 5;
    private const int RewardMax = 15;
    [SerializeField] private float _smokeOffsetZ = -1.5f;
    [SerializeField] private float _nextLevelDelay = 1.0f; //todo: actually instead of this should be toss coins animation length;

    private void Start()
    {
        if (!box0) box0 = GameObject.Find("Box_0");
        if (!box1) box1 = GameObject.Find("Box_1");
        if (!box2) box2 = GameObject.Find("Box_2");
        
        _cam = Camera.main;
        _boxes = new List<GameObject> {box0, box1, box2};
        _coinsAmountText = GameObject.Find("CoinsAmountText").GetComponent<Text>();
        _coinsAmountText.GetComponent<Text>().text = "Coins: " + SaveController.Instance.Save.CoinsCount;
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
                            var effectSpawnPos = newCamPos;

                            newCamPos.z = -4;
                            effectSpawnPos.z = _smokeOffsetZ;
                            box.GetComponent<Rotator>().enabled = false;
                            this.GetComponent<SmoothCameraTranslator>().MoveTowards(newCamPos);
                            this.GetComponent<SmoothObjectRotationResetor>().RestoreBoxRotation(box);
                            
                            StartCoroutine(nameof(RunGiftReceivingSequence), box);
                        }
                    }
                }
            }
        }
    }
    
    private IEnumerator RunGiftReceivingSequence(GameObject box)
    {
        yield return new WaitForSecondsRealtime(this.GetComponent<SmoothObjectRotationResetor>().timeToRestore);

        var effectObj = Resources.Load<GameObject>("Prefabs/SmokeScreenAlter");
        var effectPos = box.transform.position;

        effectPos.z = _smokeOffsetZ;
        GameObject.Instantiate(effectObj, effectPos, Quaternion.identity);
        box.GetComponent<Animator>().enabled = true;
        box.GetComponent<Animator>().Play("DestroyBox");

        //go next button
        // vvvvvvvvvvv
        //load_scene(segmented_scene) || game controller state = level start
    }

    public void TossCoinsAnimation()
    {
        //TODO: ILYA WILL MAKE AN ANIMATION HERE!
        GameController.Instance.AddCoins(Random.Range(RewardMin, RewardMax), _coinsAmountText);
        Debug.Log("Coins tossed!");
        StartCoroutine(nameof(GoToNextLevel));
    }

    private IEnumerator GoToNextLevel()
    {
        yield return new WaitForSecondsRealtime(_nextLevelDelay);

        SceneManager.sceneLoaded += GameController.Instance.SetUpLoadedScene;
        SceneManager.LoadScene(1);
    }
}

//TODO: SkinController.cs:45 fix update method (switch-case)
