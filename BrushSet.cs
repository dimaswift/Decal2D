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

        public abstract T CreateBrush();

        public override SingleBrush GetBrush()
        { 
            return brushes.Count > 0 && brushes[0].brush != null ? brushes[0].brush.GetBrush() : null;
        }

        public override SingleBrush GetBrush(float angle)
        {
            return brushes.Count > 0 && brushes[0].brush != null ? brushes[0].brush.GetBrush(angle) : null;
        }

        public override SingleBrush GetBrush(string tag, float angle)
        {
            return brushes.Count > 0 && brushes[0].brush != null ? brushes[0].brush.GetBrush(tag, angle) : null;
        }

        public override SingleBrush GetBrush(string tag)
        {
            return brushes.Count > 0 && brushes[0].brush != null ? brushes[0].brush.GetBrush(tag) : null;
        }

        Texture2D GetIcon(T c)
        {
            if (c == null || c.brush == null) return null;
            var b = c.brush.GetBrush();
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
