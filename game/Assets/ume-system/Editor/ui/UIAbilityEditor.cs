using UnityEditor;
using UnityEngine;

namespace UME {
	
	[CustomEditor(typeof(AbilityTrigger))]
	[CanEditMultipleObjects]
	public class UIAbilityEditor : Editor
	{
		SerializedProperty button;
		SerializedProperty key;
		SerializedProperty type;
		SerializedProperty duration;
		//SerializedProperty force;
		//SerializedProperty speed;
		

		void onEnable()
		{
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			AbilityTrigger abilityTigger = (AbilityTrigger)target;
			// button = serializedObject.FindProperty("Button");
			// key = serializedObject.FindProperty("Key");

			type = serializedObject.FindProperty("type");
			duration = serializedObject.FindProperty("duration");

			//force = serializedObject.FindProperty("force");
			//speed = serializedObject.FindProperty("speed");

			EditorGUILayout.PropertyField(type, new GUIContent("Type"));
			EditorGUILayout.PropertyField(duration, new GUIContent("Duration"));

            // switch(abilityTigger.type)
            // {
            //     case AbilityTriggerType.Fly:
			// 		// EditorGUILayout.PropertyField(button, new GUIContent("Controller Button"));
			// 		// EditorGUILayout.PropertyField(key, new GUIContent("Key"));
			// 		EditorGUILayout.PropertyField(force, new GUIContent("force"));
            //         EditorGUILayout.PropertyField(speed, new GUIContent("speed"));
            //         break;
				
			// 	case AbilityTriggerType.Invincible:
			// 		// EditorGUILayout.PropertyField(button, new GUIContent("Controller Button"));
			// 		// EditorGUILayout.PropertyField(key, new GUIContent("Key"));
			// 		EditorGUILayout.PropertyField(force, new GUIContent("force"));
            //         break;

            // default:
            //         Debug.Log("Invalid UI Trigger Type");
            //         break;
            // }
			serializedObject.ApplyModifiedProperties();
		}
	}

}