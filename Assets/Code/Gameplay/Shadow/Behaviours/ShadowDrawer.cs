using System;
using System.Collections.Generic;
using Code.Gameplay.Shadow.Behaviours.Config;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay.Shadow.Behaviours
{
    public class ShadowDrawer : MonoBehaviour
    {
        [Range(0.5f, 2f)] public float WarpFactor = 1f;
        [Range(0.1f, 5f)] public float Width = 1f;
        [Range(0f, 2f)] public float OffSet = 1f;

        public float PositionBySurface = 0.1f;
        public ShadowDisplayTypeId ShadowDisplayTypeId;
        public SpriteRenderer SpriteRenderer;
        public Color Color;
        public Material ShadowMaterial;
        public CameraShadowProjector CameraShadowProjector;
        public LightShadowProjector LightShadowProjector;

        private Texture2D _texture2D;
        private IShadowProjector _shadowProjector;

        private void OnValidate()
        {
            Cleanup();
            Draw();
        }

        [ContextMenu("Draw")]
        public void Draw()
        {
            if (ShadowDisplayTypeId == ShadowDisplayTypeId.Light)
                _shadowProjector = LightShadowProjector;
            else if (ShadowDisplayTypeId == ShadowDisplayTypeId.Camera)
                _shadowProjector = CameraShadowProjector;
            else
                throw new ArgumentException();
            
            var mesh = CreateMesh();
            var commandBuffer = new CommandBuffer();
            var material = CreateMaterial();
            var position = new Vector3(
                SpriteRenderer.transform.position.x,
                PositionBySurface,
                SpriteRenderer.transform.position.z + OffSet);
            
            _shadowProjector.Draw(commandBuffer, mesh, position, material, new Vector3(Width, 1f, 1f));
        }

        [ContextMenu("Cleanup")]
        public void Cleanup() => _shadowProjector?.Cleanup();

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
