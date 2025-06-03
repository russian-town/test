using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class ShadowDrawer : MonoBehaviour
    {
        private readonly List<IShadowCaster> _shadowCasters = new();
        
        [Range(0.1f, 5f)] [SerializeField] private float _width = 1f;
        [SerializeField] private LightShadowProjector _lightShadowProjector;

        private void OnValidate() => Draw();

        [ContextMenu("Draw")]
        public void Draw()
        {
            if(_shadowCasters.Count == 0)
                return;
            
            Cleanup();
            var commandBuffer = new CommandBuffer();
            
            foreach (var shadowable in _shadowCasters)
            {
                _lightShadowProjector.Draw(
                    commandBuffer,
                    new Vector3(_width, 1f, 1f),
                    shadowable);
            }
        }

        public void AddShadowCaster(IShadowCaster shadowCaster)
        {
            if(_shadowCasters.Contains(shadowCaster))
                return;

            _shadowCasters.Add(shadowCaster);
        }
        
        [ContextMenu("Cleanup")]
        public void Cleanup() => _lightShadowProjector?.Cleanup();
    }
}
