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

        protected bool m_dirty = false;

        [HideInInspector]
        [SerializeField]
        protected Texture2D m_decal;
        Transform m_transform;
        Color32[] m_defaultColors;
        bool m_initialized;

        public const string DECAL_PROP_NAME = "_DecalTex";
        public abstract Renderer canvasRenderer { get; }
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
            if (m_initialized)
                return;
            m_transform = transform;
            if(Application.isPlaying)
            {
                DecalPool.instance.RegisterDecal(this);
                m_defaultColors = m_decal.GetPixels32();
            }
            m_initialized = true;
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
            if (m_dirty) return;
            bool isDirty = false;
            m_decalMaterial = DecalPool.instance.PickMaterial(this, out isDirty);
            m_decal = m_decalMaterial.GetTexture(DECAL_PROP_NAME) as Texture2D;
            if (isDirty)
                Clear();
            m_dirty = true;
            canvasRenderer.sharedMaterial = m_decalMaterial;
        }

        protected virtual Vector2 GetUV(Vector2 localPoint, Brush brush)
        {
            var bounds = meshBounds;
            var uv = new Vector2();
            var zeroUV = bounds.center - bounds.extents;
            uv.x = Helper.Remap(localPoint.x, zeroUV.x, zeroUV.x + bounds.size.x, 0f, 1f);
            uv.y = Helper.Remap(localPoint.y, zeroUV.y, zeroUV.y + bounds.size.y, 0f, 1f);
            return uv;
        }

        public void PlaceBrush(Brush brush, Vector2 point, float lerp = 1f, Color color = default(Color))
        {
            var localPoint = cachedTransform.InverseTransformPoint(point);
            var bounds = meshBounds;
            localPoint.z = 0;
            if (!bounds.Contains(localPoint))
                return;
            if (!dirty)
                SetDirty();
            var decalWidth = m_decal.width;
            var decalHeight = m_decal.height;
            var b = brush.GetBrush(gameObject.tag);
            var brushWidth = b.width;
            var brushHeigth = b.height;
            var uv = GetUV(localPoint, b);

            var xStart = (int) ((uv.x * decalWidth) - (brushWidth * .5f));
            var yStart = (int) ((uv.y * decalHeight) - (brushHeigth * .5f));

            var pixelCount = b.points.Length;
            bool replaceColor = color != default(Color);
            for (int i = 0; i < pixelCount; i++)
            {
                var p = b.points[i];
                var x = xStart + p.x;
                var y = yStart + p.y;
                var old = m_decal.GetPixel(x, y);
                if(replaceColor)
                    color.a = p.color.a;
                var c = Color.Lerp(old, replaceColor ? color : p.color, lerp * p.color.a);
                if (p.color.a < .3f)
                    c = old;
                c.a += old.a * 2;
                m_decal.SetPixel(x, y, c);
            }

            m_decal.Apply();
        }
      
    }

}
