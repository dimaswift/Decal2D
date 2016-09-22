using UnityEngine;
using System.Collections;
using HandyUtilities;

namespace Decal2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteDecalCanvas : DecalCanvas
    {
        public Brush brush;

        [SerializeField]
        [HideInInspector]
        float m_decalSize = 1f;
        [SerializeField]
        [HideInInspector]
        float m_actualDecalSize = 0;
        SpriteRenderer m_spriteRenderer;
        Vector2 m_uvOffset;

        public sealed override Vector2 uvOffset
        {
            get
            {
                return m_uvOffset;
            }
        }

        public float decalSize
        {
            get
            {
                return m_decalSize;
            }
            set
            {
                m_decalSize = value;
            }
        }

        public float actualDecalSize { get { return m_actualDecalSize; } }

        public override Renderer canvasRenderer
        {
            get
            {
                return m_spriteRenderer;
            }
        }

        public override Bounds meshBounds
        {
            get
            {
                return m_spriteRenderer.sprite.bounds;
            }
        }

        public override void Init()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            if (m_spriteRenderer.sharedMaterial != null && m_spriteRenderer.sharedMaterial.HasProperty(DECAL_PROP_NAME))
                m_decal = m_spriteRenderer.sharedMaterial.GetTexture(DECAL_PROP_NAME) as Texture2D;
            base.Init();
          
        }

        void CalculateUVOffset()
        {
            var sprite = GetComponent<SpriteRenderer>().sprite;
            var offset = new Vector2();
            var scale = new Vector2();
            var textureSize = new Vector2(sprite.texture.width, sprite.texture.height);
            var spriteSize = new Vector2(sprite.rect.width, sprite.rect.height);
            scale.x = textureSize.x / spriteSize.x;
            scale.y = textureSize.y / spriteSize.y;
            offset.x = -sprite.rect.x;
            offset.y = -sprite.rect.y;
            offset.x = Helper.Remap(offset.x, 0, sprite.rect.width, 0f, 1f);
            offset.y = Helper.Remap(offset.y, 0, sprite.rect.height, 0f, 1f);
            decalMaterial.SetTextureOffset(DECAL_PROP_NAME, offset);
            decalMaterial.SetTextureScale(DECAL_PROP_NAME, scale);
        }

        public void RescaleDecal(Texture2D newDecal)
        {
            m_actualDecalSize = decalSize;
            m_decal = newDecal;
            decalMaterial.SetTexture(DECAL_PROP_NAME, newDecal);
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                PlaceBrush(brush, cachedTransform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)), 1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Clear();
            }
        }

        public override void SetDirty()
        {
            if (m_dirty) return;
        
            if (Application.isPlaying)
            {
                if(m_useMaterialPool)
                {
                    bool isDirty = false;
                    m_decalMaterial = DecalPool.instance.PickMaterial(this, out isDirty);
                    m_decal = m_decalMaterial.GetTexture(DECAL_PROP_NAME) as Texture2D;
                    if (isDirty)
                        Clear();
                }
                else
                {
                    if (m_decalMaterial != null)
                    {
                        m_decalMaterial = Instantiate(m_decalMaterial);
                        m_decal = Instantiate(m_decal);
                        m_decalMaterial.SetTexture(DECAL_PROP_NAME, m_decal);
                    }
                    else return;
                }
                CalculateUVOffset();
            }
            base.SetDirty();
        }

    }
}
