using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;
using System.Collections.Generic;

namespace Decal2D
{
    public abstract class BrushSetEditor<T> : ReordableList<IBrushBinding> where T : IBrushBinding
    {
        public abstract BrushSet<T> brushSet { get; }

        protected override float elementHeight
        {
            get
            {
                return 50;
            }
        }

        protected override string headerName
        {
            get
            {
                return "Brushes";
            }
        }

        protected override IList list
        {
            get
            {
                return brushSet.brushes;
            }
        }

        public override void OnInspectorGUI()
        {
            DoLayoutList();
            if(reorderableList.index >= 0 && reorderableList.index < brushSet.brushes.Count && brushSet.brushes[reorderableList.index].brush != null)
            {
                SingleBrushEditor.DrawBrushLayout(brushSet.brushes[reorderableList.index].brush.GetBrush());
            }
        }

        protected override IBrushBinding GetNewItem()
        {
            return brushSet.CreateBrush();
        }

        public void DrawIconInCorner(Rect rect, Brush brush)
        {
            if(brush)
            {
                var single = brush.GetBrush();
                if (single != null)
                {
                    var brushIconAspect = single.iconPreview.width / single.iconPreview.height;
                    var w = brushIconAspect * (rect.height - 10);
                    var texRect = new Rect(rect.width - (w + 5), rect.y + 5,
                        w, rect.height - 10);
                    GUI.DrawTexture(texRect, single.iconPreview, ScaleMode.ScaleToFit);
                }
            }
        }

        protected override void OnDrawItem(Rect rect, int index, bool active, bool focused)
        {
            var bind = brushSet.brushes[index];
            if(bind != null)
            {
                GUI.Label(new Rect(rect.x + 5, rect.y + 5, 40, 16), "Brush");
                
                brushSet.brushes[index].brush = EditorGUI.ObjectField(new Rect(rect.x + 45, rect.y + 5, 200, 16),
                             brushSet.brushes[index].brush,
                            typeof(SingleBrush),
                            false) as SingleBrush;

                DrawIconInCorner(rect, bind.brush);
                if (GUI.changed)
                    EditorUtility.SetDirty(brushSet);
            }
        }
    }

}
