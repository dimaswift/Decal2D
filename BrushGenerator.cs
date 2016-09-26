using UnityEngine;
using System.Collections;
using HandyUtilities;

namespace Decal2D
{
    public class BrushGenerator : RenderTextureCapture
    {
        [SerializeField]
        Brush m_brush;
        [SerializeField]
        Transform m_transformToRotate;
        [SerializeField]
        float m_angleOffset;
        [SerializeField]
        int m_steps;
        [SerializeField]
        string m_brushName = "NewBrush";

        public Brush brush { get { return m_brush; } }
        public Transform transformToRotate { get { return m_transformToRotate; } }
        public float angleOffset { get { return m_angleOffset; } }
        public int steps { get { return m_steps; } }
        public string brushName { get { return m_brushName; } }
    }

}
