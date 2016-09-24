using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;
using System;
using UnityEditorInternal;

namespace Decal2D
{
    [CustomEditor(typeof(TagBindBrush))]
    public class TagBindBrushEditor : BrushSetEditor<TagBrushBinding>
    {
        public override BrushSet<TagBrushBinding> brushSet
        {
            get
            {
                return (TagBindBrush) target;
            }
        }

        protected override void OnDrawItem(Rect rect, int index, bool active, bool focused)
        {
            var pair = brushSet.brushes[index];
            if (pair == null) return;
            var brush = pair.brush;
            GUI.Label(new Rect(rect.x + 5, rect.y + 5, 40, 16), "Brush");
            GUI.Label(new Rect(rect.x + 5, rect.y + 25, 40, 16), "Tag");
            pair.tag = EditorGUI.TextField(new Rect(rect.x + 45, rect.y + 25, 180, 16), pair.tag);
            var b = EditorGUI.ObjectField(new Rect(rect.x + 45, rect.y + 5, 195, 16),
                        pair.brush,
                        typeof(Brush),
                        false) as Brush;
            if(b != target)
            {
                pair.brush = b;
            }
            if (rect.width > 300)
            {
                DrawIconInCorner(rect, brush);
            }
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }

}
