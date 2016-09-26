using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace Decal2D
{
    [CustomEditor(typeof(BrushGenerator))]
    public class BrushGeneratorEditor : RenderCaptureEditor
    {
        BrushGenerator cam { get { return (BrushGenerator) target; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_angleOffset"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_steps"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_brush"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_transformToRotate"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_brushName"));
            if (cam.brush)
            {
                GUI.enabled =  cam.transformToRotate;
             
                if (GUILayout.Button("Generate Radial Brush"))
                {
                    GenerateRadialBrushSet();
                }
                if (GUILayout.Button("Generate Random Brush Set"))
                {
                    GenerateRadialBrushSet();
                }
                GUI.enabled = true;
            }

            if (GUILayout.Button("Add Brush"))
            {
                AddBrush(cam.brushName);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }

    

        void ClearBrushSet<T>(BrushSet<T> brushSet) where T : IBrushBinding
        {
            brushSet.brushes.Clear();
        }

        void GenerateRadialBrushSet()
        {
            if (cam.brush is RadialBrush == false) return;
            var radialBrush = cam.brush as RadialBrush;
            radialBrush.brushes.Clear();
            GenerateBrushesFromRotation((brush, anglle) =>
            {
                radialBrush.brushes.Add(new BrushAnglePair() { brush = brush, angle = anglle });
            });
            EditorUtility.SetDirty(radialBrush);  
        }

        void GenerateRandomBrushSet()
        {
            if (cam.brush is RandomBrushSet == false) return;
            var randomBrushSet = cam.brush as RandomBrushSet;
            randomBrushSet.brushes.Clear();
            GenerateBrushesFromRotation((brush, anglle) =>
            {
                randomBrushSet.brushes.Add(new SingleBrushBinding() { brush = brush });
            });
            EditorUtility.SetDirty(randomBrushSet);
        }


        SingleBrush GenerateSingleBrush(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            var texture = cam.Capture(cam.scale);
            var brush = CreateInstance<SingleBrush>();
            AssetDatabase.CreateAsset(brush, path);
            brush.Generate(texture, new Rect(0, 0, texture.width, texture.height), 1);
            DestroyImmediate(texture, true);
            return AssetDatabase.LoadAssetAtPath<SingleBrush>(path);
        }

        string CheckBrushPath(string path)
        {
            var folder = System.IO.Path.GetDirectoryName(path);
            while (AssetDatabase.LoadAssetAtPath<Brush>(path) != null)
            {
                var num = System.Text.RegularExpressions.Regex.Match(path, @"(\d+)\.asset$").Groups[1].Value;
                int order = 0;
                int.TryParse(num, out order);
                path = string.Format("{0}/{1}_{2}.asset", folder, cam.brushName, order + 1);
            }
            return path;
        }

        void AddBrush(string name)
        {
            if (cam.brush is RandomBrushSet)
            {
                var randomBrushSet = cam.brush as RandomBrushSet;
                var path = AssetDatabase.GetAssetPath(cam.brush);
                var newPath = System.IO.Path.GetDirectoryName(path) + "/" + cam.brushName + ".asset";
                var brush = GenerateSingleBrush(CheckBrushPath(newPath));
                randomBrushSet.brushes.Add(new SingleBrushBinding() { brush = brush });
            }
            else if (cam.brush is RadialBrush)
            {
                var randomBrushSet = cam.brush as RadialBrush;
                var path = AssetDatabase.GetAssetPath(cam.brush);
                var z = cam.transformToRotate.localEulerAngles.z;
                var newPath = System.IO.Path.GetDirectoryName(path) + "/" + cam.brushName + "_" + z + ".asset";
                var brush = GenerateSingleBrush(CheckBrushPath(newPath));
                randomBrushSet.brushes.Add(new BrushAnglePair() { brush = brush, angle = z });
            }
            else
            {
                string path = "";
                if (cam.brush == null)
                {
                    path = EditorUtility.SaveFilePanelInProject("Select folder", cam.brushName, "asset", "");
                    GenerateSingleBrush(path);
                }
                else
                {
                    path = AssetDatabase.GetAssetPath(cam.brush);
                    var newPath = System.IO.Path.GetDirectoryName(path) + "/" + cam.brushName + ".asset";
                    GenerateSingleBrush(CheckBrushPath(newPath));
                }
            }
            if(cam.brush)
                EditorUtility.SetDirty(cam.brush);
        }

        void GenerateBrushesFromRotation(System.Action<SingleBrush, int> onBrushCreated)
        {
            var angleStep = 360 / cam.steps;
            var startEuler = cam.transformToRotate.localEulerAngles;
            var startAngle = 0;
            var offset = startEuler.z;

            for (int i = 0; i < cam.steps; i++)
            {
                cam.transformToRotate.localEulerAngles = new Vector3(0, 0, startAngle + offset);
                var path = AssetDatabase.GetAssetPath(cam.brush);
                var brush = GenerateSingleBrush(System.IO.Path.GetDirectoryName(path) + "/" + cam.brush.name + "_" + startAngle + ".asset");
                onBrushCreated(brush, startAngle);
                startAngle += angleStep;
            }
            cam.transformToRotate.localEulerAngles = startEuler;
            EditorUtility.SetDirty(cam.brush);
        }


    }

}
