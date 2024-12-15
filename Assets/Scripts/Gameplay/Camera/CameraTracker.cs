using UnityEngine;

namespace Gameplay.Camera
{
    public class CameraTracker : MonoBehaviour
    {
        private Transform _target;
    
        public void SetTarget(Transform target)
            => _target = target;

        private void Update()
        {
            if (_target == null) return;

            transform.position = new Vector3(
                _target.position.x, _target.position.y, transform.position.z);
        }
    }
}
