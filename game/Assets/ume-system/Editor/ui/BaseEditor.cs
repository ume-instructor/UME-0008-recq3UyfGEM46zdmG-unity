using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UME
{     
     public abstract class ScriptlessEditor : Editor
     {
         private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
         
         public override void OnInspectorGUI()
         {
             serializedObject.Update();
     
             DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
     
             serializedObject.ApplyModifiedProperties();
         }
     }

    // [CustomEditor(typeof(AbilityTrigger))]
    // public class AbilityTriggerEditor : ScriptlessEditor
    // {}
    // [CustomEditor(typeof(AudioTrigger))]
    // public class AudioTriggerEditor : ScriptlessEditor
    // {}
    // [CustomEditor(typeof(ComponentTrigger))]
    // public class ComponentTriggerEditor : ScriptlessEditor
    // {}
    // [CustomEditor(typeof(CreateTrigger))]
    // public class CreateTriggerEditor : ScriptlessEditor
    // {}
    // [CustomEditor(typeof(DeleteTrigger))]
    // public class DeleteTriggerEditor : ScriptlessEditor
    // {}
    // [CustomEditor(typeof(PickupTrigger))]
    // public class PickupTriggerEditor : ScriptlessEditor
    // {}
    // [CustomEditor(typeof(RotationTrigger))]
    // public class RotationTriggerEditor : ScriptlessEditor
    // {}
    // [CustomEditor(typeof(ScaleTrigger))]
    // public class ScaleTriggerEditor : ScriptlessEditor
    // {}   
    // [CustomEditor(typeof(SwitchTrigger))]
    // public class SwitchTriggerEditor : ScriptlessEditor
    // {}   
    // [CustomEditor(typeof(TeleportTrigger))]
    // public class TeleportTriggerEditor : ScriptlessEditor
    // {}
    // //move   
    // [CustomEditor(typeof(KeyForce))]
    // public class KeyForceEditor : ScriptlessEditor
    // {}



}