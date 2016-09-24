using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;

namespace Decal2D
{
    [CustomEditor(typeof(SingleBrush))]
    public class SingleBrushEditor : Editor
    {
        SingleBrush brush { get { return (SingleBrush) target; } }

        public override void OnInspectorGUI()
        {
            var rect = EditorGUILayout.GetControlRect();
            DrawBrush(brush, rect);
        }

        public static void DrawBrush(SingleBrush brush, Rect rect)
        {
            if (brush == null) return;
            var s = brush.sprite;
            brush.sprite = EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width - 100, 16), "Sprite", brush.sprite, typeof(Sprite), true) as Sprite;
            if (s != brush.sprite)
                brush.currentScale = 0;
            rect.y += 20;
            GUI.Label(new Rect(rect.x, rect.y, rect.width, 16), brush.GetInfo());
            rect.y += 20;
            brush.scale = EditorGUI.Slider(new Rect(rect.x, rect.y, rect.width - 100, 16), "Scale", brush.scale, 0.001f, 1f);
            GUI.enabled = brush.scale != brush.currentScale;
            if (GUI.Button(new Rect(rect.width - 55, rect.y, 50, 16), "Apply"))
            {
                brush.Generate(brush.sprite, brush.scale); 
                EditorUtility.SetDirty(brush);
            }
            rect.y += 20;
            if (brush.iconPreview != null)
            {
                GUI.DrawTexture(new Rect(rect.x, rect.y, brush.iconPreview.width, brush.iconPreview.height), brush.iconPreview, ScaleMode.ScaleToFit);
                rect.y += brush.iconPreview.height + 10;
            } 
        }


        public static void DrawBrushLayout(SingleBrush brush)
        {
            if (brush == null) return;
   
            GUILayout.Label(brush.GetInfo());
            var s = brush.sprite;
            brush.sprite = EditorGUILayout.ObjectField(brush.sprite, typeof(Sprite), true) as Sprite;
            if (s != brush.sprite)
                brush.currentScale = 0;
            brush.scale = EditorGUILayout.Slider("Scale", brush.scale, 0.001f, 1f);
            GUI.enabled = brush.scale != brush.currentScale;
            if (GUILayout.Button("Apply"))
            {
                brush.Generate(brush.sprite, brush.scale);
                EditorUtility.SetDirty(brush);
            }
            GUI.enabled = true;
            var rect = EditorGUILayout.GetControlRect();
            if (brush.iconPreview != null)
            {
                EditorGUI.DrawTextureTransparent(new Rect(rect.x, rect.y, brush.iconPreview.width, brush.iconPreview.height), brush.iconPreview, ScaleMode.ScaleToFit);
                GUILayout.Space(brush.iconPreview.height + 10);
            }
        }
    }

}
