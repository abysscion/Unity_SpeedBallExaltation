/* using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
// ReSharper disable InconsistentNaming

public class DelayedEventsScript : MonoBehaviour
{
    public List<EventDelayPair> EventDelayPairs;

    private void Start()
    {
        foreach (var eventDelayPair in EventDelayPairs)
            StartCoroutine(InvokeDelayed(eventDelayPair));
    }

    private IEnumerator InvokeDelayed(EventDelayPair pair)
    {
        var timer = pair.Delay;

        do
        {
            timer -= Time.deltaTime * (1 / Time.timeScale);
            pair.Delay = timer;
            yield return null;
        } while (timer > 0);
        pair.Delay = 0;
        pair.unityEvent.Invoke();
    }

    [Serializable]
    public class EventDelayPair
    {
        public UnityEvent unityEvent;
        public float Delay;
    }
}

[CustomEditor(typeof(DelayedEventsScript))]
public class ExampleInspector : Editor
{
    private SerializedProperty EventDelayPairs;
    private ReorderableList list;
    
    private DelayedEventsScript _exampleScript;

    private void OnEnable()
    {
        _exampleScript = (DelayedEventsScript)target;

        EventDelayPairs = serializedObject.FindProperty("EventDelayPairs");

        list = new ReorderableList(serializedObject, EventDelayPairs)
        {
            draggable = true,
            displayAdd = true,
            displayRemove = true,
            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "DelayedEvents");
            },
            drawElementCallback = (rect, index, sel, act) =>
            {
                var element = EventDelayPairs.GetArrayElementAtIndex(index);
                var unityEvent = element.FindPropertyRelative("unityEvent");
                var delay = element.FindPropertyRelative("Delay");
                
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), delay);
                rect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(unityEvent)), unityEvent);
            },
            elementHeightCallback = index =>
            {
                var element = EventDelayPairs.GetArrayElementAtIndex(index);
                var unityEvent = element.FindPropertyRelative("unityEvent");
                var height = EditorGUI.GetPropertyHeight(unityEvent) + EditorGUIUtility.singleLineHeight;

                return height;
            }
        };
    }

    public override void OnInspectorGUI()
    {
        DrawScriptField();
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField()
    {
        // Disable editing
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_exampleScript), typeof(DelayedEventsScript), false);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();
    }
}

*/