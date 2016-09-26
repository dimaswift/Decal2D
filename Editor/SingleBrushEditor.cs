using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;

namespace Decal2D
{
    [CustomEditor(typeof(SingleBrush))]
    [CanEditMultipleObjects]
    public class SingleBrushEditor : Editor
    {
        SingleBrush brush { get { return (SingleBrush) target; } }

        public override void OnInspectorGUI()
        {
            var rect = EditorGUILayout.GetControlRect();
            DrawBrush(brush, rect);
        }

        public static void DrawBrush(Brush brush, Rect rect)
        {
            if (brush == null) return;
            var singleBrush = brush.GetBrushSafe();
            var s = singleBrush.sprite;
            singleBrush.sprite = EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width - 100, 16), "Sprite", singleBrush.sprite, typeof(Sprite), true) as Sprite;
            if (s != singleBrush.sprite)
                singleBrush.currentScale = 0;
            rect.y += 20;
            GUI.Label(new Rect(rect.x, rect.y, rect.width, 16), singleBrush.GetInfo());
            rect.y += 20;
            singleBrush.scale = EditorGUI.Slider(new Rect(rect.x, rect.y, rect.width - 100, 16), "Scale", singleBrush.scale, 0.001f, 1f);
            GUI.enabled = singleBrush.scale != singleBrush.currentScale && singleBrush.sprite != null;
            if (GUI.Button(new Rect(rect.width - 55, rect.y, 50, 16), "Apply"))
            {
                singleBrush.Generate(singleBrush.sprite, singleBrush.scale); 
            }
            GUI.enabled = true;
            rect.y += 20;
            singleBrush.color = EditorGUI.ColorField(new Rect(rect.x, rect.y, rect.width - 100, 16), "Color", singleBrush.color);
            rect.y += 20;
            singleBrush.effect = EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width - 100, 16), "Effect", singleBrush.effect, typeof(Effect), true) as Effect;
            rect.y += 20;
            if (singleBrush.iconPreview != null)
            {
                GUI.DrawTexture(new Rect(rect.x, rect.y, singleBrush.iconPreview.width, singleBrush.iconPreview.height), singleBrush.iconPreview, ScaleMode.ScaleToFit);
                rect.y += singleBrush.iconPreview.height + 10;
            } 
            if(GUI.changed)
                EditorUtility.SetDirty(singleBrush);
        }


        public static void DrawBrushLayout(Brush brush)
        {
            if (brush == null) return;
            var singleBrush = brush.GetBrushSafe();
            if (singleBrush == null) return;
            GUILayout.Label(singleBrush.GetInfo());
            var s = singleBrush.sprite;
            singleBrush.sprite = EditorGUILayout.ObjectField(singleBrush.sprite, typeof(Sprite), true) as Sprite;
            if (s != singleBrush.sprite)
                singleBrush.currentScale = 0;
            singleBrush.scale = EditorGUILayout.Slider("Scale", singleBrush.scale, 0.001f, 1f);
            singleBrush.color = EditorGUILayout.ColorField("Color", singleBrush.color);
            singleBrush.effect = EditorGUILayout.ObjectField("Effect", singleBrush.effect, typeof(Effect), true) as Effect;
            GUI.enabled = singleBrush.scale != singleBrush.currentScale && singleBrush.sprite != null;
            if (GUILayout.Button("Apply"))
            {
                singleBrush.Generate(singleBrush.sprite, singleBrush.scale);
                EditorUtility.SetDirty(singleBrush);
            }
            GUI.enabled = true;
            var rect = EditorGUILayout.GetControlRect();
            if (singleBrush.iconPreview != null)
            {
                EditorGUI.DrawTextureTransparent(new Rect(rect.x, rect.y, singleBrush.iconPreview.width, singleBrush.iconPreview.height), singleBrush.iconPreview, ScaleMode.ScaleToFit);
                GUILayout.Space(singleBrush.iconPreview.height + 10);
            }
        }
    }

}
