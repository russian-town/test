using UnityEngine;

namespace Code.Gameplay.Shadow.Behaviours
{
    public interface IShadowCaster
    {
        Vector3 Pivot { get; }
        Mesh Mesh { get; }
        Material Material { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 Scale { get; }
        void SetProjectRotation(Quaternion projectRotation);
        void SetLight(Light light);
        void SetForward(Vector3 forward);
    }
}
