using UnityEditor;
using UnityEngine;
namespace UME
{

    [CustomEditor(typeof(UITrigger))]
    [CanEditMultipleObjects]
    public class UITriggerEditor : Editor
    {

        SerializedProperty type;
        SerializedProperty value;
        SerializedProperty duration;
        
        SerializedProperty activate;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            UITrigger uiTrigger = (UITrigger)target;
            type = serializedObject.FindProperty("type");
            value = serializedObject.FindProperty("value");
            duration = serializedObject.FindProperty("duration");
            activate = serializedObject.FindProperty("activate");
            EditorGUILayout.PropertyField(type, new GUIContent("Type"));
            EditorGUILayout.PropertyField(activate, new GUIContent("ActivationTag"));
            //EditorGUILayout.TagField("Activation Tag", activate);
            switch(uiTrigger.type)
            {
                case UITriggerType.message:

                    if(uiTrigger.value == null)
                        uiTrigger.value="hello";
                    if(uiTrigger.duration == 0)
                        uiTrigger.duration=1;
                    EditorGUILayout.PropertyField(value, new GUIContent("Message"));
                    EditorGUILayout.PropertyField(duration, new GUIContent("Duration"));
                    break;

			    case UITriggerType.health:
                    if(uiTrigger.value == null)
                        uiTrigger.value="0";
                    EditorGUILayout.PropertyField(value, new GUIContent("Health"));
                    break;

			    case UITriggerType.score:
                    if(uiTrigger.value == null)
                        uiTrigger.value="0";
                    EditorGUILayout.PropertyField(value, new GUIContent("Score"));
                    break;

			    case UITriggerType.time:
                    if(uiTrigger.value == null)
                        uiTrigger.value="0";
                    EditorGUILayout.PropertyField(value, new GUIContent("Time"));
                    break;

			    case UITriggerType.win:
                    if(uiTrigger.value == null)
                        uiTrigger.value="winner...";
                    EditorGUILayout.PropertyField(value, new GUIContent("Message"));
                    break;

			    case UITriggerType.lose:
                    if(uiTrigger.value == null)
                        uiTrigger.value="sorry...";
                    EditorGUILayout.PropertyField(value, new GUIContent("Message"));
                    break;

            default:
                    Debug.Log("Invalid UI Trigger Type");
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

}