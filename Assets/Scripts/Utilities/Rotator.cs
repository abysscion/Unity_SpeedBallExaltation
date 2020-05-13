using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    public class Rotator : MonoBehaviour
    {
        public float xRotator;
        public float yRotator;
        public float zRotator;
        public bool xLock;
        public bool yLock;
        public bool zLock;

        private void Start()
        {
            xRotator = Math.Abs(xRotator) < 0.0001f ? Random.Range(-90, 90) : xRotator;
            yRotator = Math.Abs(yRotator) < 0.0001f ? Random.Range(-90, 90) : yRotator;
            zRotator = Math.Abs(zRotator) < 0.0001f ? Random.Range(-90, 90) : zRotator;
        }

        private void Update()
        {
            this.transform.Rotate(xLock ? 0 : xRotator * Time.deltaTime,
                yLock ? 0 : yRotator * Time.deltaTime, 
                zLock ? 0 : zRotator * Time.deltaTime);
        }
    }
}
