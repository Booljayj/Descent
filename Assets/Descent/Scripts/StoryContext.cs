using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class StoryContext : MonoBehaviour {
	public bool logChanges;

	[SerializeField] Hashtable context = new Hashtable();

	[System.Serializable]
	public class ContextEvent : UnityEvent<string, int> {}
	public ContextEvent OnContextChanged = new ContextEvent();
	
	public bool Contains(string id) {
		return context.Contains(id);
	}
	
	public void Clear() {
		context.Clear();
	}

	public void Set(string id, int value) {
		if (context.Contains(id)) {
			context[id] = value;
			if (logChanges) Debug.Log(string.Format("Context Changed: {0} = {1}", id, value));
		} else {
			context.Add(id, value);
			if (logChanges) Debug.Log(string.Format("Context Added: {0} = {1}", id, value));
		}

		OnContextChanged.Invoke(id, value);
	}

	public int Get(string id) {
		if (context.Contains(id)) {
			return (int)context[id];
		} else {
			return 0;
		}
	}
}