using UnityEngine;
using System.Collections.Generic;
using HandyUtilities;

namespace Decal2D
{
    [CreateAssetMenu(fileName ="randomBrushSet", menuName ="Decal2D/Random Brush Set", order = 1)]
    public class RandomBrushSet : BrushSet<BrushContainer>, ICustomEditorIconDrawer
    {
        [SerializeField]
        List<BrushContainer> m_brushes = new List<BrushContainer>();
        public override List<BrushContainer> brushes
        {
            get
            {
                return m_brushes;
            }
        }

        public override BrushContainer CreateBrush()
        {
            if (m_brushes.Count > 0)
            {
                var last = m_brushes.LastItem();
                return new BrushContainer() { brush = last.brush };
            }
            else return new BrushContainer() { };
        }

        Texture2D GetIcon(BrushContainer c)
        {
            if (c == null || c.brush == null) return null;
            var b = c.brush.GetBrush();
            return b != null ? b.editorIcon : null;
        }

    }
    [System.Serializable]
    public class BrushContainer : IBrushBinding
    {
        [SerializeField]
        private SingleBrush m_brush;

        public Brush brush
        {
            get { return m_brush; }
            set { if(value is SingleBrush) m_brush = (SingleBrush) value; }
        }

    }

}
