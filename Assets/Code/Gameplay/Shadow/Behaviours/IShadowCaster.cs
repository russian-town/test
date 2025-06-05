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
        void SetupTrigger(Vector3 position);
    }
}
