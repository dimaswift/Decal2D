using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;

namespace Decal2D
{
    [CustomEditor(typeof(Brush))]
    public class BrushEditor : Editor
    {
        Brush brush { get { return (Brush) target; } }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var rect = EditorGUILayout.GetControlRect();
            EditorGUI.DrawTextureTransparent(new Rect(rect.x, rect.y, brush.iconPreview.width, brush.iconPreview.height), brush.iconPreview, ScaleMode.ScaleToFit);
            GUILayout.Space(brush.iconPreview.height + 10);
            brush.scale = EditorGUILayout.Slider("Scale", brush.scale, 0.001f, 1f);
            if(brush.scale != brush.currentScale)
            {
                if (GUILayout.Button("Rescale"))
                {
                    brush.Generate(brush.source, brush.scale);
                    EditorUtility.SetDirty(brush);
                }
            }
        }

        [MenuItem("Tools/Decal2D/Create Brush")]
        static void CreateBrush()
        {
            BrushSpritePicker.Open();
        }

        public static Brush Create(string path, Sprite sprite, float scale)
        {
            var brush = HandyEditor.CreateScriptableObjectAsset<Brush>(sprite.name, path);
            brush.Generate(sprite, scale);
            EditorUtility.SetDirty(brush);
            Selection.activeObject = brush;
            return brush;
        }
    }

    public class BrushSpritePicker : EditorWindow
    {
        Sprite sprite;
        float size = 1f;
        public static void Open()
        {
            var w = GetWindow<BrushSpritePicker>(true, "Brush Sprite Picker", true);
            w.minSize = new Vector2(300, 100);
            w.maxSize = new Vector2(300, 100);
            w.Show();
            HandyEditor.CenterOnMainWin(w);
        }

        public void OnGUI()
        {
            EditorGUILayout.Space();
            sprite = EditorGUILayout.ObjectField(sprite, typeof(Sprite), true) as Sprite;
            EditorGUILayout.Space();
            size = EditorGUILayout.Slider("Size", size, 0.001f, 1f);
            if (sprite != null && GUILayout.Button("Create"))
            {
                var path = EditorUtility.SaveFilePanelInProject("Save brush", "brush", "asset", "", "Assets");
                BrushEditor.Create(path, sprite, size);
                Close();
            }
        }
    }

}
