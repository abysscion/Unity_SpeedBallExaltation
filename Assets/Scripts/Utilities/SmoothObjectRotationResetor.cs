using UnityEngine;

namespace Utilities
{
    public class SmoothObjectRotationResetor : MonoBehaviour
    {
        public float timeToRestore;
    
        private GameObject _targetObject;
        private float _rotationStep;
        private bool _shouldRestore;

        private void Start()
        {
            timeToRestore = timeToRestore <= 0.0f ? 1.0f : timeToRestore;
        }

        private void Update()
        {
            if (!_shouldRestore)
                return;

            var x = Quaternion.Angle(_targetObject.transform.rotation, Quaternion.identity);
            
            if (x <= 0.1f)
            {
                _shouldRestore = false;
                _targetObject.transform.rotation = Quaternion.identity;
                return;
            }
            _targetObject.transform.rotation = Quaternion.RotateTowards(_targetObject.transform.rotation,
                Quaternion.identity, _rotationStep * Time.deltaTime);
        }

        public void RestoreBoxRotation(GameObject target)
        {
            _shouldRestore = true;
            _targetObject = target;
            _rotationStep = Quaternion.Angle(Quaternion.identity, _targetObject.transform.rotation) / timeToRestore;
        }
    }
}
