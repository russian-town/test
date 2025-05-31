using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class ShadowDrawer : MonoBehaviour
    {
        [Range(0.1f, 5f)] public float Width = 1f;
        [Range(0f, 2f)] public float OffSet = 1f;

        public float PositionBySurface = 0.1f;
        public LightShadowProjector LightShadowProjector;
        public List<Shadowable> Shadowables;

        private Texture2D _texture2D;

        private void OnValidate()
        {
            if(LightShadowProjector
                   .Light
                   .GetCommandBuffers(LightShadowProjector.LightEvent)
                   .Length > 0)
                return;
            
            Cleanup();
            Draw();
        }

        [ContextMenu("Draw")]
        public void Draw()
        {
            foreach (var shadowable in Shadowables)
            {
                var commandBuffer = new CommandBuffer();
                var position = new Vector3(
                    shadowable.transform.position.x,
                    PositionBySurface,
                    shadowable.transform.position.z + OffSet);

                LightShadowProjector.Draw(
                    commandBuffer,
                    shadowable.Mesh,
                    position,
                    shadowable.Material, 
                    new Vector3(Width, 1f, 1f));
            }
        }

        [ContextMenu("Cleanup")]
        public void Cleanup() => LightShadowProjector?.Cleanup();
    }
}
