using UnityEngine;

namespace StoryReading {
	public class StoryBehaviour : MonoBehaviour {
		//story loading
		public virtual void OnStoryLoaded(StoryReader reader) {}
		public virtual void OnStoryStart() {}
		public virtual void OnStoryEnd() {}

		//story progress
		public virtual void OnStoryBeforeContinue() {}
		public virtual void OnStoryAfterContinue() {}
		public virtual void OnStoryText(string text, bool useSeparator, bool isFirstText) {}
		public virtual void OnStoryOption(string optionText, int optionIndex) {}
	}
}