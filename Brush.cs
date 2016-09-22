﻿using UnityEngine;
using System.Collections;
using HandyUtilities;

namespace Decal2D
{
    public class Brush : ScriptableObject, ICustomEditorIcon
    {
        public Texture2D editorIcon { get { return m_iconPreview; } }
        public float editorIconSize { get { return 50; } }
        public int width { get { return m_width; } }
        public int heigth { get { return m_heigth; } }
        public Point[] points { get { return m_points; } }
        [ReadOnly]
        public float currentScale;
        [HideInInspector]
        public Sprite source;
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

        public void SaveTexture(string path)
        {
            var t = ToTexture();
            var b = t.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, b);
        }

        public void Recalculate(float scale)
        {
            Generate(source, scale);
        }

        public Color GetColor(int i)
        {
            return m_points[i].color;
        }

        public void Generate(Sprite sprite, float scale)
        {
  
            currentScale = scale;
            source = sprite;
            var rect = source.rect;
            m_width = (int) rect.width;
            m_heigth = (int) rect.height;
       
            int i = 0;
            var texture = sprite.texture;
            var xStart = (int) rect.x;
            var yStart = (int) rect.y;
            var tmpTexture = new Texture2D(m_width, m_heigth, TextureFormat.ARGB32, true);

#if UNITY_EDITOR
            var imp = (UnityEditor.TextureImporter) UnityEditor.AssetImporter.GetAtPath(UnityEditor.AssetDatabase.GetAssetPath(texture)) as UnityEditor.TextureImporter;
            if (!imp.isReadable)
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
            if(scale < 1)
            {
                m_width = (int) (m_width * scale);
                m_heigth = (int) (m_heigth * scale);
                if (sprite.texture.filterMode == FilterMode.Point)
                    TextureResizing.Point(tmpTexture, m_width, m_heigth);
                else TextureResizing.Bilinear(tmpTexture, m_width, m_heigth);
            }
            m_points = new Point[m_width * m_heigth];
            i = 0;
            for (int y = 0; y < m_heigth; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    var p = tmpTexture.GetPixel(x, y);
                    if(p.a > .1f)
                    {
                        m_points[i] = new Point(x, y, tmpTexture.GetPixel(x, y));
                        i++;
                    }
                }
            }
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