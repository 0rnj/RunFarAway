using UnityEngine;

namespace CodeBase.Gameplay.Controllers
{
    public sealed class CameraController : MonoBehaviour
    {
        private Transform _followTarget;
        private bool _isFollowTargetSet;

        public void SetFollowTarget(Transform followTarget)
        {
            _followTarget = followTarget;
            _isFollowTargetSet = _followTarget != null;
        }

        private void Update()
        {
            if (_isFollowTargetSet)
            {
                transform.position = Vector3.forward * _followTarget.position.z;
            }
        }
    }
}