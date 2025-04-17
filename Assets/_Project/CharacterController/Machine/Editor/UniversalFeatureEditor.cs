using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class UniversalFeatureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default inspector first
        base.OnInspectorGUI();

        MonoBehaviour mb = (MonoBehaviour)target;
        System.Type type = mb.GetType();

        // Try to find a private or public field named "features" of type FeatureItemContainer
        FieldInfo featuresField = type.GetField("features", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        if (featuresField == null || featuresField.FieldType != typeof(FeatureItemContainer))
            return;

        FeatureItemContainer container = featuresField.GetValue(mb) as FeatureItemContainer;

        if (container?.Modules == null || container.Modules.Length == 0)
        {
            EditorGUILayout.HelpBox("No Character Modules available.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Character Modules", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        foreach (var module in container.Modules)
        {
            if (module == null) continue;

            module.active = EditorGUILayout.ToggleLeft(module.Name, module.active);
        }

        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            // Ensure Unity saves the change
            EditorUtility.SetDirty(mb);
        }
    }
}