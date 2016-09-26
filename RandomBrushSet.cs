using UnityEngine;
using System.Collections.Generic;
using HandyUtilities;

namespace Decal2D
{
    [CreateAssetMenu(fileName ="randomBrushSet", menuName ="Decal2D/Random Brush Set", order = 1)]
    public class RandomBrushSet : BrushSet<SingleBrushBinding>, ICustomEditorIconDrawer
    {
        [SerializeField]
        List<SingleBrushBinding> m_brushes = new List<SingleBrushBinding>();
        public override List<SingleBrushBinding> brushes
        {
            get
            {
                return m_brushes;
            }
        }

        public override SingleBrushBinding CreateBrush()
        {
            if (m_brushes.Count > 0)
            {
                var last = m_brushes.LastItem();
                return new SingleBrushBinding() { brush = last.brush };
            }
            else return new SingleBrushBinding() { };
        }

        public override SingleBrush GetBrush()
        {
            return m_brushes.Random().brush.GetBrush();
        }

        public override SingleBrush GetBrush(int order)
        {
            return m_brushes.Random().brush.GetBrush(order);
        }

        public override SingleBrush GetBrush(string tag, float angle, int order)
        {
            return m_brushes.Random().brush.GetBrush(tag, angle, order);
        }

        public override SingleBrush GetBrush(string tag, int order)
        {
            return m_brushes.Random().brush.GetBrush(tag, order);
        }

        public override SingleBrush GetBrush(float angle, int order)
        {
            return m_brushes.Random().brush.GetBrush(angle, order);
        }

    }
    [System.Serializable]
    public class SingleBrushBinding : IBrushBinding
    {
        [SerializeField]
        private Brush m_brush;

        public Brush brush
        {
            get { return m_brush; }
            set { m_brush = value; }
        }
    }
}
