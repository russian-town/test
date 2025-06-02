using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class Shadowable : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public Material SourceMaterial;
        public float WarpFactor;
        public Vector3 Pivot;

        public Mesh Mesh;
        public Material Material;

        private Texture2D _texture2D;

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

        private void OnValidate()
        {
            if (SpriteRenderer == null || SourceMaterial == null)
                return;

            Pivot = new Vector3(
                transform.position.x - SpriteRenderer.sprite.bounds.size.x / 2f,
                transform.position.y - SpriteRenderer.sprite.bounds.size.y / 2f,
                transform.position.z);

            if (Mesh == null)
                Mesh = CreateMesh();

            if (Material == null)
                Material = CreateMaterial();
        }

        private Mesh CreateMesh()
        {
            var sprite = SpriteRenderer.sprite;
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
            _texture2D = SpriteRenderer.sprite.texture;
            var newTexture2D = new Texture2D(_texture2D.width, _texture2D.height);
            var newColors = new Color[newTexture2D.width * newTexture2D.height];

            for (var y = 0; y < newTexture2D.height; y++)
            {
                for (var x = 0; x < newTexture2D.width; x++)
                {
                    float xFrac = x * 1f / (newTexture2D.width - 1f);
                    float yFrac = y * 1f / (newTexture2D.height - 1f);

                    float warpXFrac = Mathf.Pow(xFrac, WarpFactor);
                    float warpYFrac = Mathf.Pow(yFrac, WarpFactor);

                    var color = _texture2D.GetPixelBilinear(warpXFrac, warpYFrac);
                    var index = y * newTexture2D.width + x;

                    if (color.a != 0)
                        newColors[index] = Color.white;
                }
            }

            newTexture2D.SetPixels(newColors);
            newTexture2D.Apply();
            SourceMaterial.mainTexture = newTexture2D;
            return new Material(SourceMaterial);
        }
    }
}
