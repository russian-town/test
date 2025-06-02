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
            var offset  = shadowable.Position - shadowable.Pivot;
            var rotationVector = offset/* * Light.transform.eulerAngles.y * Mathf.Deg2Rad*/;
            var position = rotationVector + shadowable.Pivot;
            var direction = shadowable.Position - Light.transform.position;
            var axis = Vector3.Cross(direction - shadowable.Position, position - shadowable.Position);
            
            var matrix = Matrix4x4.TRS(
                position,
                Quaternion.LookRotation(axis.normalized + shadowable.Position, Vector3.up),
                scale);
            
            commandBuffer.DrawMesh(shadowable.Mesh, matrix, shadowable.Material);
            Light.AddCommandBuffer(LightEvent, commandBuffer);
        }

        public void Cleanup() => Light.RemoveAllCommandBuffers();
    }
}
