using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class ShadowCaster : MonoBehaviour, IShadowCaster
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _sourceMaterial;
        [SerializeField] private float _warpFactor;

        private Texture2D _texture2D;

        public Vector3 Pivot { get; private set; }

        public Vector3 Scale => new(
            _spriteRenderer.sprite.bounds.size.x,
            _spriteRenderer.sprite.bounds.size.y,
            1f);

        public Quaternion ProjectRotation { get; private set; }
        public Light Light { get; private set; }
        public Mesh Mesh { get; private set; }
        public Material Material { get; private set; }
        public Vector3 Forward { get; private set; }
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

        private void OnValidate()
        {
            if (_spriteRenderer == null || _sourceMaterial == null)
                return;

            Pivot = new Vector3(
                transform.position.x - _spriteRenderer.sprite.bounds.size.x / 2f,
                transform.position.y - _spriteRenderer.sprite.bounds.size.y / 2f,
                transform.position.z);

            if (Mesh == null)
                Mesh = CreateMesh();

            if (Material == null)
                Material = CreateMaterial();
        }

        public void SetProjectRotation(Quaternion projectRotation) => ProjectRotation = projectRotation;
        public void SetForward(Vector3 forward) => Forward = forward;
        public void SetLight(Light light) => Light = light;

        private Mesh CreateMesh()
        {
            var sprite = _spriteRenderer.sprite;
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var triangles = new int[sprite.triangles.Length];

            foreach (var vertex in sprite.vertices)
                vertices.Add(new Vector3(vertex.x, 0f, vertex.y));

            for (var i = 0; i < triangles.Length; i++)
                triangles[i] = sprite.triangles[i];

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles;
            mesh.uv = sprite.uv;

            return mesh;
        }

        private Material CreateMaterial()
        {
            _texture2D = _spriteRenderer.sprite.texture;
            var newTexture2D = new Texture2D(_texture2D.width, _texture2D.height);
            var newColors = new Color[newTexture2D.width * newTexture2D.height];

            for (var y = 0; y < newTexture2D.height; y++)
            {
                for (var x = 0; x < newTexture2D.width; x++)
                {
                    float xFrac = x * 1f / (newTexture2D.width - 1f);
                    float yFrac = y * 1f / (newTexture2D.height - 1f);

                    float warpXFrac = Mathf.Pow(xFrac, _warpFactor);
                    float warpYFrac = Mathf.Pow(yFrac, _warpFactor);

                    var color = _texture2D.GetPixelBilinear(warpXFrac, warpYFrac);
                    var index = y * newTexture2D.width + x;

                    if (color.a != 0)
                        newColors[index] = Color.white;
                }
            }

            newTexture2D.SetPixels(newColors);
            newTexture2D.Apply();
            _sourceMaterial.mainTexture = newTexture2D;
            return new Material(_sourceMaterial);
        }
    }
}
