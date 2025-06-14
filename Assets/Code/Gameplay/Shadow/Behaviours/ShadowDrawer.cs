using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class ShadowDrawer : MonoBehaviour
    {
        [Range(0.1f, 5f)] [SerializeField] private float _width = 1f;
        [SerializeField] private List<ShadowCaster> _shadowCasters = new();
        [SerializeField] private LightShadowProjector _lightShadowProjector;

        private void OnValidate() => Draw();

        [ContextMenu("Draw")]
        public void Draw()
        {
            if (_shadowCasters.Count == 0)
                return;

            Cleanup();
            var commandBuffer = new CommandBuffer();

            foreach (var shadowCaster in _shadowCasters)
            {
                var localScale = shadowCaster.transform.localScale;
                
                _lightShadowProjector.Draw(
                    commandBuffer,
                    new Vector3(localScale.x * _width, localScale.y, localScale.z),
                    shadowCaster);
            }
        }

        [ContextMenu("Cleanup")]
        public void Cleanup() => _lightShadowProjector?.Cleanup();
    }
}
