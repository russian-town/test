using UnityEngine;

namespace Code.Gameplay.Shadow.Behaviours
{
    [RequireComponent(typeof(ShadowCaster))]
    public class PhysicShadow : MonoBehaviour
    {
        [SerializeField] private ShadowCaster _shadowCaster;

        private Vector3 _direction;
        private Vector3 _center;

        private void Update()
        {
            _direction = GetDirection();
            _center = Quaternion.Euler(_direction) * _shadowCaster.Position;

            if (Physics.BoxCast(
                    _center,
                    new Vector3(_shadowCaster.Scale.x, 1f, _shadowCaster.Scale.y) / 2f,
                    GetDirection(),
                    out var hit,
                    _shadowCaster.ProjectRotation,
                    1f))
            {
                if (hit.transform.TryGetComponent(out IShadowReceiver shadowReceiver))
                {
                    shadowReceiver.Receive();
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_center, GetDirection());
            Gizmos.DrawWireCube(
                _center + GetDirection(),
                new Vector3(_shadowCaster.Scale.x, 1f, _shadowCaster.Scale.y) / 2f);
        }

        private Vector3 GetDirection() => _shadowCaster.Forward + _shadowCaster.Light.transform.forward;
    }
}
