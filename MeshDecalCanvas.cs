using UnityEngine;
using System.Collections;
using System;

namespace Decal2D
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class MeshDecalCanvas : DecalCanvas
    {
        public Brush brush;
        MeshRenderer m_meshRenderer;
        MeshFilter m_meshFilter;

        public sealed override Renderer canvasRenderer
        {
            get
            {
                return m_meshRenderer; 
            }
        }

        public sealed override Bounds meshBounds
        {
            get
            {
                return m_meshFilter.sharedMesh.bounds;
            }
        }

        public override void Init()
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
            m_meshFilter = GetComponent<MeshFilter>();
            if(Application.isPlaying)
            {
                SetDecal(Instantiate(m_meshRenderer.sharedMaterial.GetTexture(DECAL_PROP_NAME)) as Texture2D);
                m_meshRenderer.material.SetTexture(DECAL_PROP_NAME, decal);
            }
            base.Init();
        }

        void Update()
        {
            if(Input.GetMouseButton(0))
            {
            //    PlaceBrush(brush, cachedTransform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)), 1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Clear();
            }
        }

    }

}
