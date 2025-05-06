using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MovementVector2))]
public class MovementVector2Drawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty additive = property.FindPropertyRelative("Additive");
        SerializedProperty multiplicative = property.FindPropertyRelative("Multiplicative");
        
        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        
        Vector2 computed = additive.vector2Value * multiplicative.vector2Value;
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.Vector2Field(labelRect, label, computed);
        EditorGUI.EndDisabledGroup();
    }
}


[CustomPropertyDrawer(typeof(Velocity))]
public class VelocityDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty value = property.FindPropertyRelative("Value");
        
        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.Vector2Field(labelRect, label, value.vector2Value);
        EditorGUI.EndDisabledGroup();
    }
}