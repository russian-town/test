using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Shadow
{
    public class ShadowDrawer : MonoBehaviour
    {
        [Range(0.5f, 2f)] public float WarpFactor = 1f;
        
        public SpriteRenderer SpriteRenderer;
        public Transform Light;
        public Color Color;
        public Material ShadowMaterial;

        private Texture2D _texture2D;

        [ContextMenu("Draw")]
        public void Draw()
        {
            var sprite = SpriteRenderer.sprite;
            _texture2D = sprite.texture;

            Mesh mesh = new Mesh();
            var vertices = new List<Vector3>();
            var triangles = new int[sprite.triangles.Length];

            foreach (var vertex in sprite.vertices)
                vertices.Add(new Vector3(vertex.x, 0f, vertex.y));

            for (int i = 0; i < triangles.Length; i++)
                triangles[i] = sprite.triangles[i];

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles;
            mesh.uv = sprite.uv;

            var material = CreateMaterial();
            CreateShadowObject(mesh, material);
        }

        private void CreateShadowObject(Mesh newMesh, Material material)
        {
            GameObject empty = new GameObject("Shadow");
            var meshRenderer = empty.AddComponent<MeshRenderer>();
            var meshFilter = empty.AddComponent<MeshFilter>();
            var boxCollider = empty.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            newMesh.RecalculateNormals();

            meshFilter.mesh = newMesh;
            meshRenderer.sharedMaterial = material;
            
            var zPosition = (transform.position - Light.position).magnitude;
            meshFilter.transform.eulerAngles = new Vector3(0f, GetRotation(transform.position, Light.position), 0f);
            
            meshFilter.transform.position = new Vector3(
                meshFilter.transform.position.x,
                meshFilter.transform.position.y,
                zPosition);
        }

        private Material CreateMaterial()
        {
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

                    if (color.a != 0)
                        newColors[y * newTexture2D.width + x] = Color;
                }
            }

            newTexture2D.SetPixels(newColors);
            newTexture2D.Apply();

            ShadowMaterial.mainTexture = newTexture2D;
            return new Material(ShadowMaterial);
        }

        private float GetRotation(Vector3 origin, Vector3 point)
        {
            var direction = (point - origin).normalized;
            return Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        }
    }
}
