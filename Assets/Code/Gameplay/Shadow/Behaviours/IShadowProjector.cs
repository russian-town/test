using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public interface IShadowProjector
    {
        void Draw(CommandBuffer commandBuffer, Mesh mesh, Vector3 position, Material material, Vector3 scale);
        void Cleanup();
    }
}
