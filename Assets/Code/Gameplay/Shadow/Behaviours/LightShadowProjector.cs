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
            var direction = Light.transform.position - shadowable.transform.position;
            var position = shadowable.transform.position +
                           Quaternion.Euler(0f, 0f, Light.transform.eulerAngles.y) * direction;
            var rotation = Quaternion.LookRotation(shadowable.transform.position, Vector3.up);
            
            commandBuffer.DrawMesh(
                shadowable.Mesh,
                Matrix4x4.TRS(shadowable.transform.position, rotation * shadowable.transform.rotation, scale),
                shadowable.Material);
            
            Light.AddCommandBuffer(LightEvent, commandBuffer);
        }
        
        public void Cleanup() => Light.RemoveAllCommandBuffers();
        
        private Vector3 GetPosition(Vector3 normalizeDirection, float length, Vector3 position)
        {
            var rotateAxis = Vector3.Cross(Vector3.up, normalizeDirection);
            float angle = Vector3.Angle(rotateAxis, normalizeDirection);
            return position * angle;
        }

        private Quaternion GetRotation(Vector3 direction, float angle) =>
            Quaternion.LookRotation(new Vector3(0f, 0f, direction.x), Vector3.forward);
    }
}
