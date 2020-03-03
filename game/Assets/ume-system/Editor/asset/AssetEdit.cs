using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetEdit
{
    [OnOpenAssetAttribute(1)]
    public static bool step1(int instanceID, int line)
    {
        string name = EditorUtility.InstanceIDToObject(instanceID).name;
        //Debug.Log("Open Asset step: 1 (" + name + ")");
        return false; // we did not handle the open
    }

    // step2 has an attribute with index 2, so will be called after step1
    [OnOpenAssetAttribute(2)]
    public static bool step2(int instanceID, int line)
    {
        //Debug.Log("Open Asset step: 2 (" + instanceID + ")");
        return false; // we did not handle the open
    }
}