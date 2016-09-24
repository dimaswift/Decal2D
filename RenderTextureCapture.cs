using UnityEngine;
using System.Collections;
using HandyUtilities;

namespace Decal2D
{
    [RequireComponent(typeof(Camera))]
    public class RenderTextureCapture : MonoBehaviour
    {
        public float scale = 1f;
        public float min = 0, max = .5f;
        public bool pot = true;
        RenderTexture renderTexture;

        public void CaptureAndWriteToDisk(float scale, string absolutePath)
        {
            var t = Capture(scale);
            System.IO.File.WriteAllBytes(absolutePath, t.EncodeToPNG());
        }

        public Texture2D Capture(float scale)
        {
            var cam = GetComponent<Camera>();
            var spriteTexSize = cam.pixelRect;
            spriteTexSize.width *= scale;
            spriteTexSize.height *= scale;

            if (renderTexture == null)
                renderTexture = new RenderTexture((int) spriteTexSize.width, (int) spriteTexSize.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            else
            {
                renderTexture.width = (int) spriteTexSize.width;
                renderTexture.height = (int) spriteTexSize.height;
            }
            var png = new Texture2D((int) spriteTexSize.width, (int) spriteTexSize.height, TextureFormat.ARGB32, true);
            cam.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            cam.Render();
            png.ReadPixels(spriteTexSize, 0, 0);
            png.Apply();
            RenderTexture.active = null;
            cam.targetTexture = null;
            renderTexture.Release();
#if UNITY_EDITOR
            DestroyImmediate(renderTexture);
#endif
            return png;
        }
    }
}
