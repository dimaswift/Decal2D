using UnityEngine;
using System.Collections;
using HandyUtilities;

namespace Decal2D
{
    public class RadialBrushGenerator : RenderTextureCapture
    {
        [SerializeField]
        RadialBrush m_brush;
        [SerializeField]
        Transform m_spriteTransform;
        [SerializeField]
        float m_angleOffset;
        [SerializeField]
        int m_steps;

        public RadialBrush brush { get { return m_brush; } }
        public Transform spriteTransform { get { return m_spriteTransform; } }
        public float angleOffset { get { return m_angleOffset; } }
        public int steps { get { return m_steps; } }
    }

}
