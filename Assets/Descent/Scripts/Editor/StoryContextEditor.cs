using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(StoryContext))]
public class StoryContextEditor : Editor {
	StoryContext t;
	SerializedProperty onChange;

	string newkey;

	void OnEnable() {
		t = target as StoryContext;
		onChange = serializedObject.FindProperty("OnContextChanged");
	}

	public override void OnInspectorGUI () {
		List<string> keys = t.context.Keys.ToList();

		EditorGUI.indentLevel++;
		foreach (string key in keys) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(key, GUILayout.MinWidth(120f));

			t.context[key] = EditorGUILayout.IntField(GUIContent.none, t.context[key], GUILayout.Width(60f));
			if (GUI.changed) t.OnContextChanged.Invoke(key, t.context[key]);

			if (GUILayout.Button("X", GUILayout.Height(EditorGUIUtility.singleLineHeight))) {
				t.context.Remove(key);
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUI.indentLevel--;

		EditorGUILayout.BeginHorizontal();
		newkey = EditorGUILayout.TextField(GUIContent.none, newkey);
		if (string.IsNullOrEmpty(newkey) || t.context.ContainsKey(newkey)) GUI.enabled = false;
		if (GUILayout.Button("+", GUILayout.Height(EditorGUIUtility.singleLineHeight))) {
			t.context.Add(newkey, 0);
			t.OnContextChanged.Invoke(newkey, 0);
		}
		GUI.enabled = true;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(onChange);
	}
}
