using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class ShadowDrawer : MonoBehaviour
    {
        [Range(0.1f, 5f)] public float Width = 1f;

        public LightShadowProjector LightShadowProjector;
        public List<Shadowable> Shadowables;

        private void OnValidate() => Draw();

        [ContextMenu("Draw")]
        public void Draw()
        {
            Cleanup();
            var commandBuffer = new CommandBuffer();
            
            foreach (var shadowable in Shadowables)
            {
                LightShadowProjector.Draw(
                    commandBuffer,
                    new Vector3(Width, 1f, 1f),
                    shadowable);
            }
        }

        [ContextMenu("Cleanup")]
        public void Cleanup() => LightShadowProjector?.Cleanup();
    }
}
