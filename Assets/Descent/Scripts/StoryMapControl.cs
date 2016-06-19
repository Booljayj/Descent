using UnityEngine;
using Ink.Runtime;

public class StoryMapControl : MonoBehaviour {
	[SerializeField] StoryReader _reader;
	[SerializeField] string _variableName;
	[SerializeField] MapScroll _scroller;

	public void Awake() {
		_reader.OnStoryLoaded += ObserveMapVariable;
	}

	void ObserveMapVariable(Story story) {
		story.ObserveVariable(_variableName, MapVariableChanged);
	}

	void MapVariableChanged(string name, object value) {
		_scroller.ShowZone((int)value);
	}
}
