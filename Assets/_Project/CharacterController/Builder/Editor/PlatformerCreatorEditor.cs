using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(PlatformCharacterController))]
public class PlatformerCreatorEditor : Editor
{
    SelectionOption[] parts = new SelectionOption[]
    {
        new SelectionOption(
            "Movement", new ExtensionAdditionBase[]
            {
                new AccelerationExtension()
            }),
        new SelectionOption(
            "Jump", new ExtensionAdditionBase[]
            {
                new AccelerationExtension()
            }),
        new SelectionOption(
            "Dash", new ExtensionAdditionBase[]
            {
                new AccelerationExtension()
            }),
        new SelectionOption(
            "Climb", new ExtensionAdditionBase[]
            {
                new AccelerationExtension()
            }),
        new SelectionOption(
            "Sprint", new ExtensionAdditionBase[]
            {
                new AccelerationExtension()
            }),
        new SelectionOption(
            "Crouch", new ExtensionAdditionBase[]
            {
                new AccelerationExtension()
            }),
        new SelectionOption(
            "Slide", new ExtensionAdditionBase[]
            {
                new AccelerationExtension()
            }),
        new SelectionOption(
            "Glide", new ExtensionAdditionBase[]
            {
                new AccelerationExtension()
            }),
        new SelectionOption(
            "Wall", new ExtensionAdditionBase[]
            {
                new AccelerationExtension()
            })
    };
    public override void OnInspectorGUI()
    {
        PlatformCharacterController script = (PlatformCharacterController)target;
        base.OnInspectorGUI();

        foreach (var dropdown in parts)
        {
            ExtensionAdditionBase possibleExtension = dropdown.Draw();
            if (possibleExtension != null)
            {
                possibleExtension.AddExtension(script);
            }
        }
        
    }

    private class SelectionOption
{
    private int selectedIndex = 0;
    private string selectedLabel = "Select Extension Below";
    private ExtensionAdditionBase[] extensions;
    private string label;

    private AdvancedDropdownState dropdownState;
    private ExtensionDropdown dropdown; // Custom dropdown class

    private bool selectionMade = false;
    private ExtensionAdditionBase selectedExtension;

    public SelectionOption(string label, ExtensionAdditionBase[] extensions, string drop = "Select Extension Below")
    {
        this.extensions = extensions;
        this.label = label;
        this.selectedLabel = drop;

        dropdownState = new AdvancedDropdownState();

        List<string> optionNames = new List<string>();
        foreach (var extension in extensions)
        {
            optionNames.Add(extension.GetName);
        }

        dropdown = new ExtensionDropdown(dropdownState, optionNames, OnOptionSelected);
    }

    public ExtensionAdditionBase Draw()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Extension: " + label, EditorStyles.boldLabel, GUILayout.Width(180));

        Rect dropdownRect = GUILayoutUtility.GetRect(new GUIContent(selectedLabel), EditorStyles.popup);
        if (GUI.Button(dropdownRect, selectedLabel, EditorStyles.popup))
        {
            dropdown.Show(dropdownRect);
        }

        EditorGUILayout.EndHorizontal();

        if (selectionMade)
        {
            selectionMade = false;
            return selectedExtension;
        }

        return null;
    }

    private void OnOptionSelected(string selectedName)
    {
        for (int i = 0; i < extensions.Length; i++)
        {
            if (extensions[i].GetName == selectedName)
            {
                selectedExtension = extensions[i];
                selectionMade = true;
                return;
            }
        }

        selectedExtension = null;
    }
}

    private abstract class ExtensionAdditionBase
    {
        public string GetName => this.GetType().Name.RemoveLast(9);
        public abstract void AddExtension(PlatformCharacterController script);
    }
    private class AccelerationExtension : ExtensionAdditionBase
    {
        public override void AddExtension(PlatformCharacterController script)
        {
            Debug.Log("Added Acceleration Extension");
        }
    }
}
