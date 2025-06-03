using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class LightShadowProjector : MonoBehaviour
    {
        public LightEvent LightEvent;
        public Light Light;

        public void Draw(CommandBuffer commandBuffer, Vector3 scale, IShadowCaster shadowCaster)
        {
            var direction = Light.transform.position - shadowCaster.Pivot;
            var axis = Vector3.Cross(direction, shadowCaster.Pivot);
            var dot = Vector3.Dot(direction.normalized, Vector3.right);
            
            var matrix = Matrix4x4.TRS(
                shadowCaster.Position,
                shadowCaster.Rotation * Quaternion.LookRotation(axis.normalized * dot, Vector3.up),
                scale);
            
            commandBuffer.DrawMesh(shadowCaster.Mesh, matrix, shadowCaster.Material);
            Light.AddCommandBuffer(LightEvent, commandBuffer);
        }

        public void Cleanup() => Light.RemoveAllCommandBuffers();
    }
}
