using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class LightShadowProjector : MonoBehaviour
    {
        public LightEvent LightEvent;
        public Light Light;

        public void Draw(CommandBuffer commandBuffer, Vector3 scale, ShadowCaster shadowCaster)
        {
            var axis = Vector3.Cross(shadowCaster.transform.forward, shadowCaster.transform.right);
            var lookRotation = Quaternion.LookRotation(axis);
            
            var matrix = Matrix4x4.TRS(
                shadowCaster.Position,
                shadowCaster.Rotation * lookRotation,
                new Vector3(scale.x, scale.z, scale.y));
            
            commandBuffer.DrawMesh(shadowCaster.Mesh, matrix, shadowCaster.Material);
            Light.AddCommandBuffer(LightEvent, commandBuffer);
            shadowCaster.SetProjectRotation(lookRotation);
            shadowCaster.SetLight(Light);
            shadowCaster.SetForward(axis);
        }

        public void Cleanup() => Light.RemoveAllCommandBuffers();
    }
}
