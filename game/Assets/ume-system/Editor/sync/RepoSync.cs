using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;



namespace  UME
{
	[UnityEditor.InitializeOnLoad]
	static class RepoSync
	{
		static string syncScript;
		static RepoSync()
		{
			syncScript = System.Environment.GetEnvironmentVariable("UME_REPO_SYNC");
			UnityEditor.SceneManagement.EditorSceneManager.sceneSaved += OnSceneSaved;
			UnityEditor.SceneManagement.EditorSceneManager.sceneClosed += OnSceneSaved;
		}

		static void OnSceneSaved(UnityEngine.SceneManagement.Scene scene)
		{
			try{
				Sync();
			}
			catch{
				UnityEngine.Debug.Log("ERROR >> Repo Sync");
			}
		}

		static void Sync(){
			if (syncScript != null && File.Exists(syncScript)){

				ProcessStartInfo pinfo = new ProcessStartInfo();
				pinfo.WindowStyle = ProcessWindowStyle.Normal;
				pinfo.UseShellExecute = false;
				pinfo.FileName = "python";
				pinfo.Arguments = syncScript;
				pinfo.RedirectStandardOutput = true;
				pinfo.RedirectStandardError = true;
				using(Process process = Process.Start(pinfo)){
					process.WaitForExit();
				}
			}
		}


	}
}

