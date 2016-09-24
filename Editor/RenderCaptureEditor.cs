using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using HandyUtilities;

namespace Decal2D
{
    [CustomEditor(typeof(RenderTextureCapture), true)]
    public class RenderCaptureEditor : Editor
    {
        RenderTextureCapture capturerer { get { return (RenderTextureCapture) target; } }

        public override void OnInspectorGUI()
        {
            var cam = capturerer.GetComponent<Camera>();
            EditorGUILayout.MinMaxSlider(ref capturerer.min, ref capturerer.max, 0f, 5f);
            capturerer.scale = EditorGUILayout.Slider(capturerer.scale, capturerer.min, capturerer.max);
            capturerer.pot = EditorGUILayout.Toggle("Power Or Two", capturerer.pot);

            if(capturerer.pot)
            {
                var v = (int)(capturerer.scale * cam.pixelHeight);
                var d = int.MaxValue;
                float fin = cam.pixelHeight;
                foreach (var pot in GetPot(4, 2048))
                {
                    var diff = Mathf.Abs(pot - v);
                    if(diff < d)
                    {
                        d = diff;
                        fin = pot;
                    }
                }
                capturerer.scale = fin / cam.pixelHeight;
            }
        
            GUILayout.Label(string.Format("Height: {0}", capturerer.scale * cam.pixelHeight));
            GUILayout.Label(string.Format("Width: {0}", capturerer.scale * cam.pixelWidth));
            EditorUtility.SetDirty(capturerer);  
 
            if (GUILayout.Button("Capture"))
            {
                var path = EditorUtility.SaveFilePanelInProject("Save Texture", "texture", "png", "");
                if (!string.IsNullOrEmpty(path))
                {
                    var t = capturerer.Capture(capturerer.scale);
                    System.IO.File.WriteAllBytes(Helper.ConvertToAbsolutePath(path), t.EncodeToPNG());
                    AssetDatabase.ImportAsset(path);
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                }
            }
        }

        IEnumerable<int> GetPot(int from, int to)
        {
            var v = from;
            while(v < to)
            {
                v *= 2;
                yield return v;
            }
        }

        void Capture()
        {
            var cam = capturerer.GetComponent<Camera>();
            var path = EditorUtility.SaveFilePanelInProject("Save Texture", "texture", "png", "");
            if (string.IsNullOrEmpty(path)) return;
            var spriteTexSize = cam.pixelRect;
            spriteTexSize.width *= capturerer.scale;
            spriteTexSize.height *= capturerer.scale;
            var renderTexture = new RenderTexture((int)spriteTexSize.width, (int) spriteTexSize.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            var png = new Texture2D((int) spriteTexSize.width, (int) spriteTexSize.height, TextureFormat.ARGB32, true);
            cam.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            cam.Render();
            png.ReadPixels(spriteTexSize, 0, 0);
            png.Apply();
            System.IO.File.WriteAllBytes(Helper.ConvertToAbsolutePath(path), png.EncodeToPNG());
            AssetDatabase.ImportAsset(path);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            RenderTexture.active = null;
            cam.targetTexture = null;
            renderTexture.Release();
            DestroyImmediate(renderTexture);
        }

    }

}
