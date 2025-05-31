using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class LightShadowProjector : MonoBehaviour
    {
        public LightEvent LightEvent;
        public Light Light;

        private Vector3 _direction;

        public void Draw(CommandBuffer commandBuffer, Mesh mesh, Vector3 position, Material material, Vector3 scale)
        {
            _direction = (Light.transform.position - position).normalized;

            commandBuffer.DrawMesh(
                mesh,
                Matrix4x4.TRS(
                    position + GetPosition(),
                    GetRotation(position),
                    scale),
                material);
            
            Light.AddCommandBuffer(LightEvent, commandBuffer);
        }
        
        public void Cleanup() => Light.RemoveAllCommandBuffers();
        
        private Vector3 GetPosition()
        {
            var rotateAxis = Vector3.Cross(transform.forward, _direction);
            float angle = Vector3.Angle(rotateAxis, _direction);
            return Quaternion.Euler(0f, angle, 0f) * Vector3.up;
        }

        private Quaternion GetRotation(Vector3 position) =>
            Quaternion.LookRotation(GetPosition() - new Vector3(0f, position.y, 0f), transform.up);
    }
}
