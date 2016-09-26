using UnityEngine;
using System.Collections.Generic;
using HandyUtilities;
using System.Collections;

namespace Decal2D
{
    public interface IBrushSet<T>
    {
        List<T> brushes { get; }
    }

    public interface IBrushBinding
    {
        Brush brush { get; set; }
    }

    public abstract class BrushSet<T> : Brush, IBrushSet<T> where T : IBrushBinding
    {
        public abstract List<T> brushes { get; }

        public Texture2D editorIcon
        {
            get
            {
                if(brushes.Count > 0 && brushes[0] != null)
                    return brushes[0].brush.GetBrush().editorIcon;
                return null;
            }
        }

        public float editorIconSize
        {
            get
            {
                return 25;
            }
        }

        public override SingleBrush GetBrushSafe()
        {
            if (brushes.Count == 0) return null;
            var b = brushes[0];
            if (b == null) return null;
            if (b.brush == null) return null;
            return b.brush.GetBrushSafe();
        }

        public abstract T CreateBrush();

        Texture2D GetIcon(T c)
        {
            if (c == null || c.brush == null) return null;
            var b = c.brush.GetBrushSafe();
            return b != null ? b.editorIcon : null;
        }

        public void DrawEditorIcon(Rect rect)
        {
            if (rect.height < 20)
            {
                var b1Icon = GetIcon(brushes[0]);
                if (b1Icon)
                {
                    var a = b1Icon.height / b1Icon.width;
                    GUI.DrawTexture(new Rect(rect.x, rect.y, rect.height, rect.height * a), b1Icon);
                }
                return;
            }
            float p = .4f;
            if (brushes.Count > 0)
            {
                var b1Icon = GetIcon(brushes[0]);
                if (b1Icon != null)
                    GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width * p, rect.height * p), b1Icon);
            }
            if (brushes.Count > 1)
            {
                var b1Icon = GetIcon(brushes[1]);
                if (b1Icon != null)
                    GUI.DrawTexture(new Rect(rect.x + rect.width * p, rect.y, rect.width * p, rect.height * p), b1Icon);
            }
            if (brushes.Count > 2)
            {
                var b1Icon = GetIcon(brushes[2]);
                if (b1Icon != null)
                    GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height * p, rect.width * p, rect.height * p), b1Icon);
            }
            if (brushes.Count > 3)
            {
                var b1Icon = GetIcon(brushes[3]);
                if (b1Icon != null)
                    GUI.DrawTexture(new Rect(rect.x + rect.width * p, rect.y + rect.height * p, rect.width * p, rect.height * p), b1Icon);
            }
        }
    }
}
