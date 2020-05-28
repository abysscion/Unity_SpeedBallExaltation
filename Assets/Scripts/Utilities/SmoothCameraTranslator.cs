using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Utilities
{
    public class SmoothCameraTranslator : MonoBehaviour
    {
        public float timeToMove;

        private Vector3 _translateStep;
        private Vector3 _targetPoint;
        private Camera _cam;
        private bool _shouldMove;

        private void Start()
        {
            _cam = Camera.main;
            _targetPoint = _cam.transform.position;
            if (timeToMove <= 0.0f) timeToMove = 1.0f;
        }

        private void Update()
        {
            if (!_shouldMove)
                return;
            
            if ((_targetPoint - _cam.transform.position).magnitude <= 0.1f)
            {
                _cam.transform.position = _targetPoint;
                _shouldMove = false;
                return;
            }
            _cam.transform.Translate(_translateStep * Time.deltaTime);
        }
    
        public void MoveTowards(Vector3 point)
        {
            _targetPoint = point;
            _shouldMove = true;
            _translateStep = (_targetPoint - _cam.transform.position) / timeToMove;
        }
    }
}
