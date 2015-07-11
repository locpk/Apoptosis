using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad] [CustomEditor(typeof(TerrainTransparency))] public class TerrainTransparencyEditor : Editor
{
	static Dictionary<TerrainTransparency, float> targetLastMouseChangeTimes = new Dictionary<TerrainTransparency, float>(); 
	static TerrainTransparencyEditor() { EditorApplication.update += OnEditorUpdate; }
	static void OnEditorUpdate() {
		foreach (TerrainTransparency target in targetLastMouseChangeTimes.Keys.ToList())
			if (targetLastMouseChangeTimes[target] + .3f <= Time.realtimeSinceStartup) {
				target.UpdateTransparencyMap();
				targetLastMouseChangeTimes.Remove(target);
			}
	}

	new TerrainTransparency target;

	public override void OnInspectorGUI() {
		if (!target)
			target = (TerrainTransparency)base.target;

		bool changeMade = DrawDefaultInspector();
		if (GUILayout.Button("Update Terrain")) {
			target.UpdateTransparencyMap();
			target.ApplyTransparencyMap();
		}

		if (target.autoUpdateTransparencyMap && (changeMade || (new[] {EventType.ValidateCommand, EventType.Used}.Contains(Event.current.type) && Event.current.commandName == "UndoRedoPerformed")))
			target.UpdateTransparencyMap();
	}

	void OnSceneGUI() {
		if (!target) target = (TerrainTransparency)base.target;

		if (target.autoUpdateTransparencyMap && Event.current.rawType == EventType.used && Event.current.button == 0 && Selection.transforms.Any(a=>a.gameObject == target.gameObject))
			targetLastMouseChangeTimes[target] = Time.realtimeSinceStartup;
	}
}