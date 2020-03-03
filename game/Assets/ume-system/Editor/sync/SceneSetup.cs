using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace  UME
{
	[UnityEditor.InitializeOnLoad]
	static class SceneSetup
	{
		static SceneSetup()
		{
			UnityEditor.SceneManagement.EditorSceneManager.newSceneCreated += OnSceneNew;
            UnityEditor.SceneManagement.EditorSceneManager.sceneLoaded += OnSceneLoaded;
		} 
        static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            UnityEngine.Debug.Log("Loaded");
        }

		static void OnSceneNew(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.NewSceneSetup setup, UnityEditor.SceneManagement.NewSceneMode mode)
		{
                UnityEngine.Debug.Log("New");
                GameObject main_cam = Camera.main.gameObject;
                if(main_cam !=null){
                    GameObject cam = PrefabUtility.InstantiatePrefab(Resources.Load("Camera") as GameObject) as GameObject;
                    cam.name="Camera";
                    GameObject.DestroyImmediate(main_cam);
                }

		}


	}
}


