using UnityEngine;
using System.Collections.Generic;
using HandyUtilities;

namespace Decal2D
{
    [CreateAssetMenu(fileName ="Single Brush", menuName = "Decal2D/Single Brush", order = 0)]
    public class SingleBrush : Brush, ICustomEditorIcon
    {
        public int width { get { return m_width; } }
        public int height { get { return m_heigth; } }
        public Point[] points { get { return m_points; } }

        [ReadOnly]
        public float currentScale;
        public Sprite sprite;
        [SerializeField]
        [ReadOnly]
        int m_width, m_heigth;
        [SerializeField]
        [HideInInspector]
        Point[] m_points;
        [SerializeField]
        [HideInInspector]
        Texture2D m_iconPreview;
        [HideInInspector]
        public float scale = 1f;

        public string GetInfo()
        {
            return string.Format("Height: {0} Width:{1}", (int) (currentScale * m_width), (int) (currentScale * m_heigth));
        }

        public Texture2D editorIcon
        {
            get
            {
                return iconPreview;
            }
        }

        public float editorIconSize { get { return 50; } }

        public Texture2D iconPreview { get { return m_iconPreview; } }

        public Texture2D ToTexture()
        {
            var t = new Texture2D(m_width, m_heigth, TextureFormat.ARGB32, true);
            var colors = new Color[m_width * m_heigth];
            var length = colors.Length;
            for (int i = 0; i < length; i++)
            {
                colors[i] = m_points[i].color;
            }
            t.SetPixels(colors);
            t.Apply();

            return t;
        }

        public sealed override SingleBrush GetBrush(string tag) { return this; }

        public sealed override SingleBrush GetBrush(float angle) { return this; }

        public sealed override SingleBrush GetBrush(string tag, float angle) { return this; }

        public sealed override SingleBrush GetBrush() { return this; }

        public void SaveTexture(string path)
        {
            var t = ToTexture();
            var b = t.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, b);
        }

        public void Generate(Sprite sprite, float scale)
        {
            this.sprite = sprite;
            Generate(sprite.texture, sprite.rect, scale);
        }

        public void Generate(Texture2D texture, Rect rect, float scale)
        {
            currentScale = scale;
            m_width = (int) rect.width;
            m_heigth = (int) rect.height;
            int i = 0;
            var xStart = (int) rect.x;
            var yStart = (int) rect.y;
            var tmpTexture = new Texture2D(m_width, m_heigth, TextureFormat.ARGB32, true);

#if UNITY_EDITOR
            var imp = (UnityEditor.TextureImporter) UnityEditor.AssetImporter.GetAtPath(UnityEditor.AssetDatabase.GetAssetPath(texture)) as UnityEditor.TextureImporter;
            if (imp != null && !imp.isReadable)
            {
                imp.isReadable = true;
                imp.SaveAndReimport();
            }

#endif
            for (int y = 0; y < m_heigth; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    tmpTexture.SetPixel(x, y, texture.GetPixel(xStart + x, yStart + y));
                    i++;
                }
            }
            tmpTexture.Apply();
            if (scale < 1)
            {
                m_width = (int) (m_width * scale);
                m_heigth = (int) (m_heigth * scale);
                if (texture.filterMode == FilterMode.Point)
                    TextureResizing.Point(tmpTexture, m_width, m_heigth);
                else TextureResizing.Bilinear(tmpTexture, m_width, m_heigth);
            }

            List<Point> list = new List<Point>(m_width * m_heigth);

            for (int y = 0; y < m_heigth; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    var p = tmpTexture.GetPixel(x, y);
                    if (p.a > 0)
                    {
                        var point = new Point(x, y, tmpTexture.GetPixel(x, y));
                        list.Add(point);
                    }
                }
            }
            m_points = list.ToArray();
#if UNITY_EDITOR
            if (m_iconPreview != null)
            {
                DestroyImmediate(m_iconPreview, true);
            }
            m_iconPreview = tmpTexture;
            m_iconPreview.hideFlags = HideFlags.HideInHierarchy;
            UnityEditor.AssetDatabase.AddObjectToAsset(m_iconPreview, this);
#else
            DestroyImmediate(tmpTexture);
#endif
        }


        [System.Serializable]
        public struct Point
        {
            public int x, y;
            public Color color;
            public Point(int x, int y, Color color)
            {
                this.color = color;
                this.x = x;
                this.y = y;
            }
        }
    }
}
