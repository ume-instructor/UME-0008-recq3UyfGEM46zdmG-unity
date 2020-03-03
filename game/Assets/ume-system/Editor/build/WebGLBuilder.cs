using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
  
 class WebGLBuilder {
	
	private static string[] SplitPath(string mainText, string pivot)
	{
		string[] result = new string[2];
		string[] dirs = mainText.Split(Path.DirectorySeparatorChar).ToArray();
		int pivotIndex = dirs.Length;
		for (int i = 0; i < dirs.Length; i++)
		{
			if (dirs[i] == pivot)
			{
				pivotIndex = i;
				break;
			}
		}
		result[0] = string.Join(Path.DirectorySeparatorChar.ToString(), dirs.Take(pivotIndex).ToArray());
		result[1] = string.Join(Path.DirectorySeparatorChar.ToString(), dirs.Skip(pivotIndex).ToArray());
		return result;
	}
 	 private static string[] GetScenes(){
		string [] scenes;
     	scenes = (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
		if (scenes.Length == 0) {
			//grab scene files by date of lastWrite in hopes of grabbing the latest worked on first
			System.IO.FileInfo[] files;
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(Application.dataPath);
			files = directoryInfo.GetFiles ("*.unity").OrderBy (t => t.CreationTime).ToArray();
			scenes = files.Select(file => file.FullName).Reverse().ToArray();
			// make scenes relative to Assets folder
			List<string> paths = new List<string>();
			foreach (string p in scenes) {
				string[] spath = SplitPath(p,"Assets");
				paths.Add (spath[1]);
			}
			scenes = paths.ToArray();
		}
		return scenes;
	}
	[MenuItem("Build/webGL")]
     static void build() {
		string [] scenes = GetScenes ();
		BuildPipeline.BuildPlayer(scenes, System.Environment.GetEnvironmentVariable("BUILD_DIR"), BuildTarget.WebGL, BuildOptions.None);
     }
 }
