using UnityEngine;
using System.Collections.Generic;
using HandyUtilities;

namespace Decal2D
{
    [CreateAssetMenu(fileName ="radialBrushSet", menuName ="Decal2D/Radial Brush Set", order = 2)]
    public class RadialBrush : BrushSet<BrushAnglePair>, ICustomEditorIconDrawer
    {
        [SerializeField]
        Sprite m_sprite;
        [SerializeField]
        List<BrushAnglePair> m_brushes = new List<BrushAnglePair>();
        public Sprite sprite { get { return m_sprite; } }
        public override List<BrushAnglePair> brushes { get { return m_brushes; } }

        public override SingleBrush GetBrush(float angle)
        {
            var a = 360f;
            SingleBrush brush = GetBrush();
            var count = brushes.Count;
            for (int i = 0; i < count; i++)
            {
                BrushAnglePair b = m_brushes[i];
                var angleDiff = Mathf.DeltaAngle(angle, b.angle);
                if (angleDiff < a)
                {
                    brush = b.brush.GetBrush();
                    a = angleDiff;
                }
            }
            return brush;
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