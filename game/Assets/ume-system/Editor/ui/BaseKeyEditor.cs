using UnityEditor;
using UnityEngine;

namespace UME {
	
	[CustomEditor(typeof(BaseKey))]
	[CanEditMultipleObjects]
	public class BaseKeyEditor : Editor
	{
		SerializedProperty Button;
		SerializedProperty Key;
		public override void OnInspectorGUI()
		{
			BaseKey baseKey = (BaseKey)target;
			Button = serializedObject.FindProperty("Button");
			Key = serializedObject.FindProperty("Key");
			EditorGUILayout.PropertyField(Button, new GUIContent("Controller"));
			EditorGUILayout.PropertyField(Key, new GUIContent("Keyboard"));
			serializedObject.ApplyModifiedProperties();

		}
	}

}