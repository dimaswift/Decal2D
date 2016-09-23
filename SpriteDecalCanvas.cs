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
            base.Init();
        }

        void CalculateUVOffset()
        {
            var sprite = m_spriteRenderer.sprite;
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
            base.SetDirty();
            CalculateUVOffset();
        }

    }
}
