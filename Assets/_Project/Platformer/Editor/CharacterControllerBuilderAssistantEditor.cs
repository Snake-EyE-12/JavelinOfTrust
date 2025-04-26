using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CharacterController.Platformer
{
    [CustomEditor(typeof(CharacterControllerBuilderAssistant))]
    public class CharacterControllerBuilderAssistantEditor : Editor
    {
        private ControllerDropdown platformerDropdown;
        private ControllerDropdown topDownDropdown;
        private ControllerDropdown threeDDropdown;

        private void OnEnable()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly =>
                {
                    try { return assembly.GetTypes(); }
                    catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
                });

            GameObject go = ((MonoBehaviour)target).gameObject;

            platformerDropdown = new ControllerDropdown("Platformer",
                FilterTypes(go, allTypes, typeof(PlatformerPlayerBehavior)), go);

            topDownDropdown = new ControllerDropdown("Top-Down",
                FilterTypes(go, allTypes, typeof(TopDownPlayerBehavior)), go);

            threeDDropdown = new ControllerDropdown("3D",
                FilterTypes(go, allTypes, typeof(ThreeDPlayerBehavior)), go);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GameObject go = ((MonoBehaviour)target).gameObject;
            
            if (go.GetComponent<Controller>() == null)
            {
                if (GUILayout.Button("Create Controller", GUILayout.MaxWidth(150), GUILayout.MinHeight(25)))
                {
                    Undo.AddComponent<Controller>(go);
                }
                EditorGUILayout.Space(20);
            }

            EditorGUILayout.BeginHorizontal();

            platformerDropdown.Draw();
            topDownDropdown.Draw();
            threeDDropdown.Draw();

            EditorGUILayout.EndHorizontal();
        }

        private List<Type> FilterTypes(GameObject go, IEnumerable<Type> allTypes, Type baseType)
        {
            return allTypes
                .Where(t => t != null && t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
                .Where(t => go.GetComponent(t) == null)
                .ToList();
        }

        private class ControllerDropdown
        {
            private string label;
            private List<Type> controllerTypes;
            private string[] displayNames;
            private GameObject targetObject;
            private AdvancedDropdownState state = new AdvancedDropdownState();

            public ControllerDropdown(string label, List<Type> types, GameObject targetObject)
            {
                this.label = label;
                this.controllerTypes = types;
                this.targetObject = targetObject;
                this.displayNames = types.Select(CleanName).ToArray();
            }

            public void Draw()
            {
                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(150), GUILayout.MinWidth(100));

                EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.MaxWidth(150));

                if (controllerTypes.Count > 0)
                {
                    Rect rect = GUILayoutUtility.GetRect(new GUIContent("Select " + label + " Behavior"), EditorStyles.popup, GUILayout.Width(150));
                    if (GUI.Button(rect, "Select " + label, EditorStyles.popup))
                    {
                        var dropdown = new ExtensionDropdown(state, displayNames, OnDropdownItemSelected);
                        dropdown.Show(rect);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("None", GUILayout.MaxWidth(150));
                }

                EditorGUILayout.EndVertical();
                GUILayout.Space(10);
            }

            private void OnDropdownItemSelected(int index)
            {
                if (index >= 0 && index < controllerTypes.Count)
                {
                    Component addedComponent = Undo.AddComponent(targetObject, controllerTypes[index]);
                    if (addedComponent is PlayerBehavior behavior)
                    {
                        Controller controller = targetObject.GetComponent<Controller>();
                        if (controller != null)
                        {
                            // Get the private field 'initialBehavior'
                            FieldInfo field = typeof(Controller).GetField("initialBehavior", BindingFlags.NonPublic | BindingFlags.Instance);
                            if (field != null)
                            {
                                var list = field.GetValue(controller) as IList<PlayerBehavior>;
                                if (list != null && !list.Contains(behavior))
                                {
                                    list.Add(behavior);
                                }
                            }

                            // Mark the controller as dirty so Unity saves the change
                            EditorUtility.SetDirty(controller);
                        }
                    }
                }
            }

            private string CleanName(Type type)
            {
                string name = type.Name;
                int index = name.IndexOf("Control", StringComparison.OrdinalIgnoreCase);
                return index > 0 ? name.Substring(0, index) : name;
            }
        }

        public class ExtensionDropdown : AdvancedDropdown
        {
            private string[] options;
            private Action<int> onSelected;

            public ExtensionDropdown(AdvancedDropdownState state, string[] options, Action<int> onSelected)
                : base(state)
            {
                this.options = options;
                this.onSelected = onSelected;
            }

            protected override AdvancedDropdownItem BuildRoot()
            {
                var root = new AdvancedDropdownItem("Behaviors");
                for (int i = 0; i < options.Length; i++)
                {
                    root.AddChild(new AdvancedDropdownItem(options[i]) { id = i });
                }
                return root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                onSelected?.Invoke(item.id);
            }
        }
    }
}
