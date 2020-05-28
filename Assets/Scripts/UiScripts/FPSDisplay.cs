using UnityEngine;

namespace UiScripts
{
    public class FPSDisplay : MonoBehaviour
    {
        private float _deltaTime;

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;
            var rect = new Rect(w - 300, h - 70, w, h * 2 / 100);
            var mSec = _deltaTime * 1000.0f;
            var fps = 1.0f / _deltaTime;
            var text = $"{mSec:0.0} ms ({fps:0.} fps)";
            var style = new GUIStyle
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = h * 2 / 100,
                normal = {textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f)}
            };

            GUI.Label(rect, text, style);
        }
    }
}
