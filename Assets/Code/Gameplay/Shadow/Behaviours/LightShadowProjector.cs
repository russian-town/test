using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class LightShadowProjector : MonoBehaviour, IShadowProjector
    {
        public LightEvent LightEvent;
        public Light Light;

        private Vector3 _direction;

        public void Draw(CommandBuffer commandBuffer, Mesh mesh, Vector3 position, Material material)
        {
            _direction = (Light.transform.position - position).normalized;
            
            commandBuffer.DrawMesh(
                mesh,
                Matrix4x4.TRS(GetPosition(), GetRotation(position), Vector3.one),
                material);
            Light.AddCommandBuffer(LightEvent, commandBuffer);
        }

        public void Cleanup() => Light.RemoveAllCommandBuffers();

        private Vector3 GetPosition()
        {
            return Quaternion.AngleAxis(Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg, 
                Vector3.up) * Vector3.up;
        }

        private Quaternion GetRotation(Vector3 position) =>
            Quaternion.LookRotation(GetPosition() - position, transform.up);
    }
}
