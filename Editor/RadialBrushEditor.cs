using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;
using System;
using UnityEditorInternal;

namespace Decal2D
{
    [CustomEditor(typeof(RadialBrush))]
    public class RadialBrushEditor : BrushSetEditor<BrushAnglePair>
    {
        public override BrushSet<BrushAnglePair> brushSet
        {
            get
            {
                return (RadialBrush) target;
            }
        }

        public RadialBrush radialBrush
        {
            get
            {
                return (RadialBrush) target;
            }
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_sprite"));
            
            if(radialBrush.sprite)
            {
                if (GUILayout.Button("Generate"))
                {
                    Screen.SetResolution(100, 100, false);
                } 
            }
            if(GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        protected override void OnDrawItem(Rect rect, int index, bool active, bool focused)
        {
            var pair = brushSet.brushes[index] as BrushAnglePair;
            if (pair == null) return;
            var brush = pair.brush;
            GUI.Label(new Rect(rect.x + 5, rect.y + 5, 40, 16), "Brush");

            pair.angle = EditorGUI.Slider(new Rect(rect.x + 45, rect.y + 25, 180, 16), pair.angle, 0, 360);
            var b = EditorGUI.ObjectField(new Rect(rect.x + 45, rect.y + 5, 195, 16),
                        pair.brush,
                        typeof(Brush),
                        false) as Brush;
            if (b != target)
                pair.brush = b;
            if (rect.width > 300)
            {
                DrawIconInCorner(rect, brush);
            }
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }

}
