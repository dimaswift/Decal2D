using UnityEngine;
using System.Collections;
using HandyUtilities;

namespace Decal2D
{
    public abstract class DecalCanvas : MonoBehaviour
    {
        [SerializeField]
        protected Material m_decalMaterial;
        [SerializeField]
        bool m_loadOnAwake = true;
        [SerializeField]
        bool m_setDirtyOnLoad = true;
        [SerializeField]
        protected bool m_useMaterialPool = false;
        [SerializeField]
        protected bool m_dirty = false;

        [HideInInspector]
        [SerializeField]
        protected Texture2D m_decal;
        Transform m_transform;
        Color32[] m_defaultColors;

        public const string DECAL_PROP_NAME = "_DecalTex";
        public abstract Renderer canvasRenderer { get; }
        public virtual Vector2 uvOffset { get { return Vector2.zero; } }
        public abstract Bounds meshBounds { get; }
        public bool dirty { get { return m_dirty; } }
        public Transform cachedTransform { get { return m_transform; } }
        public Texture2D decal { get { return m_decal; } }

        public Material decalMaterial
        {
            get { return m_decalMaterial; }
            set { m_decalMaterial = value; }
        }
      
        void Awake()
        {
            if(m_loadOnAwake)
                Init();
        }

        public virtual void Init()
        {
            m_transform = transform;
            if(Application.isPlaying)
            {
                if(m_useMaterialPool)
                {
                    DecalPool.instance.RegisterDecal(this);
                }
                m_defaultColors = m_decal.GetPixels32();
                if(m_setDirtyOnLoad)
                {
                    SetDirty();
                }
            }
        }

        public void SetDecal(Texture2D decal)
        {
            m_decal =  decal;
        }

        public void Clear()
        {
            m_decal.SetPixels32(m_defaultColors);
            m_decal.Apply();
        }

        public virtual void SetDirty()
        {
            m_dirty = true;
            canvasRenderer.sharedMaterial = m_decalMaterial;
        }

        public void PlaceBrush(Brush brush, Vector2 localPoint, float lerp = 1f)
        {
            var bounds = meshBounds;
            if (!bounds.Contains(localPoint))
                return;
            if (!dirty)
                SetDirty();

            var decalWidth = m_decal.width;
            var decalHeight = m_decal.height;
            var brushWidth = brush.width;
            var brushHeigth = brush.heigth;
            var uv = new Vector2();

          
            var zeroUV = bounds.center - bounds.extents;
            uv.x = Helper.Remap(localPoint.x, zeroUV.x, zeroUV.x + bounds.size.x, 0f, 1f);
            uv.y = Helper.Remap(localPoint.y, zeroUV.y, zeroUV.y + bounds.size.y, 0f, 1f);

            var xStart = (int) ((uv.x * decalWidth) - (brushWidth * .5f));
            var yStart = (int) ((uv.y * decalHeight) -(brushHeigth * .5f));

            var pixelCount = brush.points.Length;

            for (int i = 0; i < pixelCount; i++)
            {
                var p = brush.points[i];
                var x = xStart + p.x;
                var y = yStart + p.y;
                var old = m_decal.GetPixel(x, y);
                var c = Color.Lerp(old, p.color, lerp * p.color.a);
                if (p.color.a < .3f)
                    c = old;
                c.a += old.a * 2;
                m_decal.SetPixel(x, y, c);
            }

            m_decal.Apply();
        }
    }

}
