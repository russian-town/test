using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Behaviours.Shadow
{
    public class ShadowDrawer : MonoBehaviour
    {
        [Range(0.5f, 2f)] public float WarpFactor = 1f;
        [Range(0.1f, 5f)] public float Size = 1f;

        public SpriteRenderer SpriteRenderer;
        public Light Light;
        public Color Color;
        public Material ShadowMaterial;

        private Texture2D _texture2D;

        [ContextMenu("Draw")]
        public void Draw()
        {
            var mesh = CreateMesh();
            var material = CreateMaterial();
            var commandBuffer = new CommandBuffer();
            var position = SpriteRenderer.transform.localToWorldMatrix;
            commandBuffer.DrawMesh(mesh, position, material);
            Light.AddCommandBuffer(LightEvent.BeforeShadowMapPass, commandBuffer);
        }

        private Mesh CreateMesh()
        {
            var sprite = SpriteRenderer.sprite;
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var triangles = new int[sprite.triangles.Length];

            foreach (var vertex in sprite.vertices)
                vertices.Add(new Vector3(vertex.x, 0f, vertex.y) * Size);

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
                        newColors[index] = Color;
                }
            }

            newTexture2D.SetPixels(newColors);
            newTexture2D.Apply();

            ShadowMaterial.mainTexture = newTexture2D;
            return new Material(ShadowMaterial);
        }
    }
}
