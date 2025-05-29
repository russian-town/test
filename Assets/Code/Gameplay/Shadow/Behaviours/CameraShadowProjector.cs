using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class CameraShadowProjector : MonoBehaviour, IShadowProjector
    {
        public Camera Camera;
        public CameraEvent CameraEvent;
        
        public float Distance => Vector3.Distance(Camera.transform.position, transform.position);
        
        public void Draw(CommandBuffer commandBuffer, Mesh mesh, Vector3 position, Material material, Vector3 scale)
        {
            commandBuffer.SetProjectionMatrix(Matrix4x4.Perspective(Camera.fieldOfView, Camera.aspect, Camera.nearClipPlane, Camera.farClipPlane));
            commandBuffer.DrawMesh(mesh, Matrix4x4.TRS(position, Quaternion.identity, scale), material);
            Camera.AddCommandBuffer(CameraEvent, commandBuffer);
        }

        public void Cleanup() => Camera.RemoveAllCommandBuffers();
    }
}
