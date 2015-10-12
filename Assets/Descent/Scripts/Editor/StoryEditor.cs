using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(Story))]
public class StoryEditor : Editor {
	Story t;

	public void OnEnable() {
		t = target as Story;
	}

	public override void OnInspectorGUI () {
		if (GUILayout.Button("Import")) {
			string path = EditorUtility.OpenFilePanel("Select Inklewriter File", Application.dataPath, "json");
			if (!string.IsNullOrEmpty(path)) {
				Hashtable table = StoryImporter.ImportInklewriter(File.ReadAllText(path));
				t.title = table["title"] as string;
				t.start = table["start"] as string;
				t.snippets = table["snippets"] as List<Story.Snippet>;
				EditorUtility.SetDirty(t);
			}
		}

		//DrawDefaultInspector();
		EditorStyles.textField.wordWrap = true;
		foreach (Story.Snippet s in t.snippets) {
			EditorGUILayout.Separator();

			EditorGUILayout.LabelField(s.id, EditorStyles.boldLabel);
			EditorGUILayout.TextArea(s.text);

			EditorGUI.indentLevel += 1;
			foreach (Story.Option o in s.options) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(o.text, GUILayout.Width(125f));
				EditorGUILayout.LabelField("=>", GUILayout.Width(35f));
				EditorGUILayout.LabelField(o.link, GUILayout.Width(140f));
				EditorGUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel -= 1;
		}
	}
}