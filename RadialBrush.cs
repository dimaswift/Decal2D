using UnityEngine;
using System.Collections.Generic;
using HandyUtilities;

namespace Decal2D
{
    [CreateAssetMenu(fileName ="radialBrushSet", menuName ="Decal2D/Radial Brush Set", order = 2)]
    public sealed class RadialBrush : BrushSet<BrushAnglePair>, ICustomEditorIconDrawer
    {
        [SerializeField]
        List<BrushAnglePair> m_brushes = new List<BrushAnglePair>();

        public override List<BrushAnglePair> brushes { get { return m_brushes; } }

        public override SingleBrush GetBrush(float angle, int order = 0)
        {
            var a = 360f;
            SingleBrush brush = m_brushes[0].brush.GetBrush(order);
            var count = brushes.Count;
            for (int i = 0; i < count; i++)
            {
                BrushAnglePair b = m_brushes[i];
                var angleDiff = Mathf.DeltaAngle(angle, b.angle);
                if (angleDiff < a)
                {
                    brush = b.brush.GetBrush(order);
                    a = angleDiff;
                }
            }
            return brush; 
        }

        public override SingleBrush GetBrush()
        {
            return m_brushes[0].brush.GetBrush();
        }

        public override SingleBrush GetBrush(int order)
        {
            return m_brushes[0].brush.GetBrush(order);
        }

        public override SingleBrush GetBrush(string tag, float angle, int order)
        {
            var a = 360f;
            SingleBrush brush = m_brushes[0].brush.GetBrush(tag, order);
            var count = brushes.Count;
            for (int i = 0; i < count; i++)
            {
                BrushAnglePair b = m_brushes[i];
                var angleDiff = Mathf.DeltaAngle(angle, b.angle);
                if (angleDiff < a)
                {
                    brush = b.brush.GetBrush(tag, order);
                    a = angleDiff;
                }
            }
            return brush;
        }

        public override SingleBrush GetBrush(string tag, int order)
        {
            return m_brushes[0].brush.GetBrush(tag, order);
        }

        public override BrushAnglePair CreateBrush()
        {
            if (m_brushes.Count > 0)
            {
                var last = m_brushes.LastItem();
                return new BrushAnglePair() { brush = last.brush, angle = last.angle };
            }
            else return new BrushAnglePair() { };
        }

    }

    [System.Serializable]
    public class BrushAnglePair : IBrushBinding
    {
        [SerializeField]
        SingleBrush m_brush;
        public Brush brush
        {
            get { return m_brush; }
            set { if(value is SingleBrush) m_brush = (SingleBrush) value; }
        }
        public float angle;
    } 
}