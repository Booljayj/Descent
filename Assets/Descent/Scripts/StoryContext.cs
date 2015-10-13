using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StoryContext : MonoBehaviour, ISerializationCallbackReceiver {
	public Dictionary<string, int> context;
	[SerializeField, HideInInspector] string[] keys;
	[SerializeField, HideInInspector] int[] values;

	[System.Serializable] public class ContextEvent : UnityEvent<string, int> {}
	public ContextEvent OnContextChanged = new ContextEvent();

	public bool Contains(string id) {
		return context.ContainsKey(id);
	}
	
	public void Clear() {
		context.Clear();
	}

	public void Set(string id, int value) {
		if (context.ContainsKey(id)) {
			context[id] = value;
		} else {
			context.Add(id, value);
		}

		OnContextChanged.Invoke(id, value);
	}

	public int Get(string id) {
		if (context.ContainsKey(id)) {
			return (int)context[id];
		} else {
			return 0;
		}
	}

	public void OnAfterDeserialize() {
		context = new Dictionary<string, int>(keys.Length);
		if (keys != null) {
			for (int i = 0; i < keys.Length; i++) {
				context.Add(keys[i], values[i]);
			}
		}
	}
	
	public void OnBeforeSerialize() {
		if (context == null) context = new Dictionary<string, int>();
		keys = context.Keys.ToArray();
		values = context.Values.ToArray();
	}
}