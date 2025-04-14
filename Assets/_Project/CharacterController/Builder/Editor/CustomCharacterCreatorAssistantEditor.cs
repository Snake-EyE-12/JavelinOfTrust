using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomCharacterCreatorAssistant))]
public class CustomCharacterCreatorAssistantEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CustomCharacterCreatorAssistant script = (CustomCharacterCreatorAssistant)target;
        base.OnInspectorGUI();
        
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("2D Platform Controller"))
        {
            if (script.gameObject.TryGetComponent(out ICharacterCreator creator))
            {
                ContainsAlready();
            }
            else
            {
                script.gameObject.AddComponent<PlatformCharacterController>();
            }
        }
        if (GUILayout.Button("Top Down Controller"))
        {
            if (script.gameObject.TryGetComponent(out ICharacterCreator creator))
            {
                ContainsAlready();
            }
            else
            {
                throw new System.NotImplementedException();
                //script.gameObject.AddComponent<PlatformCharacterCreatorAssistant>();
            }
        }
        
        GUILayout.EndHorizontal();
    }

    private void ContainsAlready()
    {
        Debug.LogError("Object already has a character creator!");
    }
}