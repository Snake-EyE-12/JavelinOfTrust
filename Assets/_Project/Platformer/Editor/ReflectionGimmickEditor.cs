using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace CharacterController.Platformer
{ 
    [CustomEditor(typeof(PlayerBehavior), true)]
    public class ReflectiveGimmickEditor : Editor
    {
        private List<(FieldInfo, SerializedProperty, List<Action>)> _fields;
        private void OnEnable()
        {
            _fields = new List<(FieldInfo, SerializedProperty, List<Action>)>();
            var so = serializedObject;
            var targetType = target.GetType();
            

            foreach (var field in targetType.GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (!typeof(Gimmick).IsAssignableFrom(field.FieldType))
                    continue;
                var prop = so.FindProperty(field.Name);
                var gimmickInstance = field.GetValue(target);
                if (gimmickInstance == null)
                    continue; 
                var methods = field.FieldType
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(mi => mi.GetCustomAttribute<PrioritizedAttribute>() != null)
                    .ToArray();

                var actions = new List<Action>();
                foreach (var mi in methods)
                {
                    if (mi.GetParameters().Length != 0 || mi.ReturnType != typeof(void))
                        throw new InvalidOperationException(
                            $"Method {mi.Name} has invalid signature. Must be parameterless void."
                        );
                    var action = (Action) Delegate.CreateDelegate(typeof(Action), gimmickInstance, mi);
                    actions.Add(action);
                }

                (gimmickInstance as Gimmick)?.SetActions(actions);
                if (prop != null)
                {
                    _fields.Add((field, prop, actions));
                }
                
                serializedObject.Update();
                var prioritiesProp = prop.FindPropertyRelative("priorities");
                prioritiesProp.arraySize = actions.Count;
                serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField("Gimmicks", EditorStyles.boldLabel);
            EditorGUILayout.Space(2);
            
            var blueStyle = new GUIStyle(EditorStyles.boldLabel);
            blueStyle.normal.textColor = new Color(0.7f, 0.8f, 0.95f);
            blueStyle.richText = true;
            

            foreach (var (field, prop, actions) in _fields)
            {
                EditorGUILayout.BeginVertical("HelpBox");

                EditorGUILayout.BeginHorizontal();

                var activeProp = prop.FindPropertyRelative("active");
                activeProp.boolValue = EditorGUILayout.Toggle(activeProp.boolValue, GUILayout.Width(18));
                GUILayout.Space(4);

                GUIStyle boldStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
                EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(field.Name), boldStyle);

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (activeProp.boolValue)
                {
                    EditorGUILayout.Space(4);
                    Rect rect = EditorGUILayout.GetControlRect(false, 1);
                    EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
                    EditorGUILayout.Space(4);
                    
                    var child = prop.Copy();
                    var end = child.GetEndProperty();
                    child.NextVisible(true);
                    
                    EditorGUILayout.LabelField("<u>Variables</u>", blueStyle);
                    
                    while (!SerializedProperty.EqualContents(child, end))
                    {
                        if (child.name != "active" && child.name != "priorities")
                        {
                            GUILayout.Space(2);
                            EditorGUILayout.PropertyField(child, includeChildren: true);
                        }
                        child.NextVisible(false);
                    }

                    if (actions.Count > 0)
                    {
                        EditorGUILayout.Space(4);
                        rect = EditorGUILayout.GetControlRect(false, 1);
                        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
                        EditorGUILayout.Space(1);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("<u>Functionality</u>", blueStyle);
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.LabelField("<u>Priority</u>", blueStyle);
                        EditorGUILayout.EndHorizontal();
                    }
                    var prioritiesProp = prop.FindPropertyRelative("priorities");
                    for (int i = 0; i < actions.Count; i++)
                    {
                        GUILayout.Space(2);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(actions[i].Method.Name);
                        var singlePriority = prioritiesProp.GetArrayElementAtIndex(i);
                        EditorGUILayout.PropertyField(singlePriority, GUIContent.none, true);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(4);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
