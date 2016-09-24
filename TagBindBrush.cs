using UnityEngine;
using System.Collections;
using HandyUtilities;
using System.Collections.Generic;

namespace Decal2D
{
    [CreateAssetMenu(fileName ="tagBindBrush", menuName ="Decal2D/Tag Bind Brush", order = 3)]
    public class TagBindBrush : BrushSet<TagBrushBinding>, ICustomEditorIconDrawer
    {
        [SerializeField]
        List<TagBrushBinding> m_brushes = new List<TagBrushBinding>();
        public override List<TagBrushBinding> brushes
        {
            get { return m_brushes; }
        }

        public override TagBrushBinding CreateBrush()
        {
            if (m_brushes.Count > 0)
            {
                var last = m_brushes.LastItem();
                return new TagBrushBinding() { brush = last.brush, tag = last.tag };
            }
            else return new TagBrushBinding() { tag = "Default" };
        }

        public override SingleBrush GetBrush(string tag)
        {
            var count = brushes.Count;
            for (int i = 0; i < count; i++)
            {
                TagBrushBinding b = m_brushes[i];
                if (b.tag == tag)
                {
                    return b.brush.GetBrush();
                }
            }
            return GetBrush();
        }

    }

    [System.Serializable]
    public class TagBrushBinding : IBrushBinding
    {
        public string tag;
        [SerializeField]
        private Brush m_brush;

        public Brush brush
        {
            get { return m_brush; }
            set { m_brush = value; }
        }
    }

}
