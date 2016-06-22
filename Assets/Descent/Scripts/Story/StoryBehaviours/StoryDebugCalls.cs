using UnityEngine;

namespace StoryReading {
	public class StoryDebugCalls : StoryBehaviour {
		[SerializeField] bool _showDebugMessages;

		//story loading
		public override void OnStoryLoaded(StoryReader reader) {
			if (_showDebugMessages) Debug.Log("OnStoryLoaded");
		}
		public override void OnStoryStart() {
			if (_showDebugMessages) Debug.Log("OnStoryStart");
		}
		public override void OnStoryEnd() {
			if (_showDebugMessages) Debug.Log("OnStoryEnd");
		}

		//story progress
		public override void OnStoryBeforeContinue() {
			if (_showDebugMessages) Debug.Log("OnStoryBeforeContinue");
		}
		public override void OnStoryAfterContinue() {
			if (_showDebugMessages) Debug.Log("OnStoryAfterContinue");
		}
		public override void OnStoryText(string text, bool useSeparator, bool isFirstText) {
			if (_showDebugMessages)
				Debug.Log(string.Format("OnStoryText({0}, {1}, {2})", text, useSeparator, isFirstText));
		}
		public override void OnStoryOption(string optionText, int optionIndex) {
			if (_showDebugMessages)
				Debug.Log(string.Format("OnStoryOption({0}, {1})", optionText, optionIndex));
		}
	}
}
