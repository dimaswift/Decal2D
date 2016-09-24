using UnityEngine;
using System.Collections;
using UnityEditor;
using HandyUtilities;

namespace Decal2D
{
    [CustomEditor(typeof(SpriteDecalCanvas))]
    public class DecalCanvasEditor : Editor
    {
        SpriteDecalCanvas canvas { get { return (SpriteDecalCanvas) target; } }

        void OnEnable()
        {
            canvas.Init();
            if (!Application.isPlaying)
            {
                if (canvas.decal != null)
                {
                    TextureImporter imp = (TextureImporter) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(canvas.decal));
                    if (imp != null && !imp.isReadable)
                    {
                        imp.isReadable = true;
                        imp.SaveAndReimport();
                    }
                }
            }
            
            EditorUtility.SetDirty(canvas);  
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            if(canvas.canvasRenderer == null)
            {
                canvas.Init();
            }
            if(canvas.decal)
            {
                canvas.decalSize = EditorGUILayout.Slider("Decal Size", canvas.decalSize, 0.1f, 1f);
                EditorGUILayout.SelectableLabel(string.Format("Decal Size W:{0}, H:{1}", canvas.decal.width, canvas.decal.height));
                if (canvas.decalSize != canvas.actualDecalSize)
                {
                    if (GUILayout.Button("Rescale Decal"))
                    {
                        RescaleDecal();
                    }
                }
            }
              
            if(canvas.decalMaterial == null)
            {
                if(GUILayout.Button("Create Prefab, Material and Decal"))
                {
                    CreateMaterial();
                }
            }
            if(EditorGUI.EndChangeCheck() && canvas != null)
                EditorUtility.SetDirty(canvas);  
        }

        void RescaleDecal()
        {
            var sprite = canvas.GetComponent<SpriteRenderer>().sprite;
          
            TextureResizing.Bilinear(canvas.decal, sprite.rect.width * canvas.decalSize, sprite.rect.height * canvas.decalSize);
            var decalPath = AssetDatabase.GetAssetPath(canvas.decal);
            System.IO.File.WriteAllBytes(Helper.ConvertToAbsolutePath(decalPath), canvas.decal.EncodeToPNG());
            AssetDatabase.ImportAsset(decalPath);
            TextureImporter imp = (TextureImporter) AssetImporter.GetAtPath(decalPath);
            imp.isReadable = true;
            imp.SaveAndReimport();
            canvas.RescaleDecal(canvas.decal);
            EditorUtility.SetDirty(canvas);  
        }

        void CreateMaterial()
        {
            var path = EditorUtility.SaveFolderPanel("Save Decal", Application.dataPath, "Assets");
            if(!string.IsNullOrEmpty(path))
            {
                var mat = new Material(Shader.Find("Decal2D/SpriteDecalUnlit"));
                var decal = SaveTextureFromSprite(canvas.GetComponent<SpriteRenderer>().sprite, true);
                TextureResizing.Bilinear(decal, canvas.decalSize);
                System.IO.File.WriteAllBytes(path + "/" + canvas.name + "_decal.png", decal.EncodeToPNG());
                var decalPath = Helper.ConvertLoRelativePath(path) + "/" + canvas.name + "_decal.png";
                AssetDatabase.ImportAsset(decalPath);
                TextureImporter imp = (TextureImporter) AssetImporter.GetAtPath(decalPath);
                imp.isReadable = true;
                imp.textureFormat = TextureImporterFormat.ARGB32;
                imp.mipmapEnabled = true;
                imp.wrapMode = TextureWrapMode.Clamp;
                imp.SaveAndReimport();
                canvas.decalMaterial = mat;
                canvas.RescaleDecal(AssetDatabase.LoadAssetAtPath<Texture2D>(decalPath));
                AssetDatabase.CreateAsset(mat, Helper.ConvertLoRelativePath(path) + "/" + canvas.name + "_material.mat");
            
                EditorUtility.SetDirty(canvas);
                var prefabPath = Helper.ConvertLoRelativePath(path) + "/" + canvas.name + ".prefab";
                var prefab = PrefabUtility.CreatePrefab(prefabPath, canvas.gameObject);
                PrefabUtility.ConnectGameObjectToPrefab(canvas.gameObject, prefab);
                Selection.activeObject = prefab;
            }
        }

        public static Texture2D SaveTextureFromSprite(Sprite sprite, bool clear)
        {
            var t = new Texture2D((int) sprite.rect.width, (int) sprite.rect.height, TextureFormat.ARGB32, true);
            var offset = new Vector2(sprite.rect.x, sprite.rect.y);
            HandyEditor.MakeTextureReadable(sprite.texture);
            for (int x = 0; x < t.width; x++)
            {
                for (int y = 0; y < t.height; y++)
                {
                    var c = sprite.texture.GetPixel((int) (x + offset.x), (int) (y + offset.y));
                    if (clear)
                        c.a = 0;
                    t.SetPixel(x, y, c);

                }
            }
            t.Apply();
            return t;
        }
    }

}
