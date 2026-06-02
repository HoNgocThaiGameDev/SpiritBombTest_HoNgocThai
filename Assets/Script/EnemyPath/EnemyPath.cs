using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Pixelplacement/EnemyPath")]
public class EnemyPath : MonoBehaviour
{
	public string pathName ="";
	public Color pathColor = Color.cyan;
	public List<Vector3> nodes = new List<Vector3>(){Vector3.zero, Vector3.zero};
	public int nodeCount;
	public static Dictionary<string, EnemyPath> paths = new Dictionary<string, EnemyPath>();
	public bool initialized = false;
	public string initialName = "";
	public bool pathVisible = true;
		
	void OnEnable(){
		RegisterPath();
	}
	
	void OnDisable(){
		string normalizedName = NormalizePathName(pathName);
		EnemyPath registeredPath;
		if (paths.TryGetValue(normalizedName, out registeredPath) && registeredPath == this){
			paths.Remove(normalizedName);
			RegisterActiveReplacement(normalizedName);
		}
	}
	
	void OnDrawGizmosSelected(){
		if(pathVisible){
			if(nodes.Count > 1){
				Gizmos.color = pathColor;
				for (int i = 0; i < nodes.Count - 1; i++){
					Gizmos.DrawLine(nodes[i], nodes[i+1]);
				}
			}	
		}
	}
	
	public static Vector3[] GetPath(string requestedName){
		EnemyPath path;
		if(TryGetActivePath(requestedName, out path)){
			return path.nodes.ToArray();
		}else{
            Debug.LogError("No active path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
			return null;
		}
	}
	
	public static Vector3[] GetPathReversed(string requestedName){
		EnemyPath path;
		if(TryGetActivePath(requestedName, out path)){
			List<Vector3>  revNodes = path.nodes.GetRange(0,path.nodes.Count);
			revNodes.Reverse();
			return revNodes.ToArray();
		}else{
			Debug.LogError("No active path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
			return null;
		}
	}

	private void RegisterPath(){
		string normalizedName = NormalizePathName(pathName);
		if (string.IsNullOrEmpty(normalizedName)){
			return;
		}

		EnemyPath registeredPath;
		if (!paths.TryGetValue(normalizedName, out registeredPath)
			|| registeredPath == null
			|| !registeredPath.isActiveAndEnabled){
			paths[normalizedName] = this;
		}
	}

	private static bool TryGetActivePath(string requestedName, out EnemyPath path){
		string normalizedName = NormalizePathName(requestedName);
		if (string.IsNullOrEmpty(normalizedName)){
			path = null;
			return false;
		}

		if (paths.TryGetValue(normalizedName, out path)
			&& path != null
			&& path.isActiveAndEnabled){
			return true;
		}

		paths.Remove(normalizedName);
		RegisterActiveReplacement(normalizedName);
		return paths.TryGetValue(normalizedName, out path)
			&& path != null
			&& path.isActiveAndEnabled;
	}

	private static void RegisterActiveReplacement(string normalizedName){
		EnemyPath[] activePaths = FindObjectsOfType<EnemyPath>();
		for (int i = 0; i < activePaths.Length; i++){
			EnemyPath activePath = activePaths[i];
			if (activePath != null
				&& activePath.isActiveAndEnabled
				&& NormalizePathName(activePath.pathName) == normalizedName){
				paths[normalizedName] = activePath;
				return;
			}
		}
	}

	private static string NormalizePathName(string requestedName){
		return string.IsNullOrEmpty(requestedName) ? string.Empty : requestedName.ToLowerInvariant();
	}
}

// Subclass to avoid breaking existing serialized GameObjects (such as prefabs/scenes) in Unity
[System.Obsolete("Use EnemyPath instead")]
public class iTweenPath : EnemyPath {}
