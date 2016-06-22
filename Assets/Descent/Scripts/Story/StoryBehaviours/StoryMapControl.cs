using UnityEngine;

namespace StoryReading {
	public class StoryMapControl : StoryBehaviour {
		[SerializeField] string _variableName;
		[SerializeField] MapScroll _scroller;

		[SerializeField, HideInInspector] StoryReader _reader;

		public override void OnStoryLoaded(StoryReader reader) {
			reader.Story.ObserveVariable(_variableName, MapVariableChanged);
		}

		void MapVariableChanged(string name, object value) {
			_scroller.ShowZone((int)value);
		}
	}
}
