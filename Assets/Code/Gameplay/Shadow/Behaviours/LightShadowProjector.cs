using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class LightShadowProjector : MonoBehaviour
    {
        public LightEvent LightEvent;
        public Light Light;

        public void Draw(CommandBuffer commandBuffer, Vector3 scale, Shadowable shadowable)
        {
            var direction = Light.transform.position - shadowable.Pivot;
            var axis = Vector3.Cross(direction, shadowable.Pivot);
            var dot = Vector3.Dot(direction.normalized, Vector3.right);
            
            var matrix = Matrix4x4.TRS(
                shadowable.Position,
                shadowable.Rotation * Quaternion.LookRotation(axis.normalized * dot, Vector3.up),
                scale);
            
            commandBuffer.DrawMesh(shadowable.Mesh, matrix, shadowable.Material);
            Light.AddCommandBuffer(LightEvent, commandBuffer);
        }

        public void Cleanup() => Light.RemoveAllCommandBuffers();
    }
}
