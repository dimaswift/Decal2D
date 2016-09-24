using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Decal2D
{
    [CustomEditor(typeof(RadialBrushGenerator))]
    public class RadialBrushGeneratorEditor : RenderCaptureEditor
    {
        RadialBrushGenerator cam { get { return (RadialBrushGenerator) target; } }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_angleOffset"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_steps"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_brush"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_spriteTransform"));
            if (cam.brush && cam.spriteTransform)
            {
                if(GUILayout.Button("Generate Brush"))
                {
                    Generate();
                }
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }

        void Generate()
        {
            var angleStep = 360 / cam.steps;
            var startEuler = cam.spriteTransform.localEulerAngles;
            var startAngle = 0;
            var offset = startEuler.z;
            cam.brush.brushes.Clear();
            for (int i = 0; i < cam.steps; i++)
            {
                cam.spriteTransform.localEulerAngles = new Vector3(0, 0, startAngle + offset);
                var texture = cam.Capture(cam.scale);
                var brush = CreateInstance<SingleBrush>();
                var path = AssetDatabase.GetAssetPath(cam.brush);
                var brushPath = System.IO.Path.GetDirectoryName(path) + "/" + cam.brush.name + "_" + startAngle + ".asset";
                AssetDatabase.CreateAsset(brush, brushPath);
                cam.brush.brushes.Add(new BrushAnglePair()
                {
                    brush = AssetDatabase.LoadAssetAtPath<SingleBrush>(brushPath),
                    angle = startAngle
                });
                brush.Generate(texture, new Rect(0, 0, texture.width, texture.height), cam.scale);
                DestroyImmediate(texture, true);
                startAngle += angleStep;
            }
            cam.spriteTransform.localEulerAngles = startEuler;
            EditorUtility.SetDirty(cam.brush);
        }

    }

}
