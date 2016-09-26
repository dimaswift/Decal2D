using UnityEngine;
using System.Collections.Generic;
using HandyUtilities;

namespace Decal2D
{
    [CreateAssetMenu(fileName = "orderedBrush", menuName = "Decal2D/Ordered Brush", order = 5)]
    public sealed class OrderedBrush : BrushSet<OrderedBrushBinding>, ICustomEditorIconDrawer
    {
        [SerializeField]
        List<OrderedBrushBinding> m_brushes = new List<OrderedBrushBinding>();

        public sealed override List<OrderedBrushBinding> brushes { get { return m_brushes; } }

        public override SingleBrush GetBrush()
        {
            return m_brushes[0].brush.GetBrush();
        }

        public override SingleBrush GetBrush(int order)
        {
            order = Mathf.Clamp(order, 0, m_brushes.Count - 1);
            return m_brushes[order].brush.GetBrush();
        }

        public override SingleBrush GetBrush(string tag, int order)
        {
            order = Mathf.Clamp(order, 0, m_brushes.Count - 1);
            return m_brushes[order].brush.GetBrush(tag, order);
        }

        public override SingleBrush GetBrush(string tag, float angle, int order)
        {
            order = Mathf.Clamp(order, 0, m_brushes.Count - 1);
            return m_brushes[order].brush.GetBrush(tag, angle, order);
        }

        public override SingleBrush GetBrush(float angle, int order)
        {
            order = Mathf.Clamp(order, 0, m_brushes.Count - 1);
            return m_brushes[order].brush.GetBrush(angle, order);
        }

        public override OrderedBrushBinding CreateBrush()
        {
            if (m_brushes.Count > 0)
            {
                var last = m_brushes.LastItem();
                return new OrderedBrushBinding() { brush = last.brush };
            }
            else return new OrderedBrushBinding() { };
        }
    }

    [System.Serializable]
    public class OrderedBrushBinding : IBrushBinding
    {
        [SerializeField]
        Brush m_brush;
        public Brush brush
        {
            get { return m_brush; }
            set { m_brush = value; }
        }
    }
}