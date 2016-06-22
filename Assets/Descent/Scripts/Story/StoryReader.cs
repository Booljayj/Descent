using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

namespace StoryReading {
	public class StoryReader : MonoBehaviour {
		[SerializeField] TextAsset _storyJSON;
		[SerializeField] bool _playOnStart;

		[SerializeField, HideInInspector] List<StoryBehaviour> _behaviours;

		[SerializeField, HideInInspector] bool _isPlaying;
		public bool isPlaying {get {return _isPlaying;}}

		Story _inkStory;
		public Story Story {get {return _inkStory;}}

		void Awake() {
			_behaviours = new List<StoryBehaviour>();
			GetComponents<StoryBehaviour>(_behaviours);
		}

		void Start() {
			if (_storyJSON) LoadStory(_storyJSON, _playOnStart);
		}

		public void LoadStory(TextAsset storyJSON, bool playImmediately) {
			_storyJSON = storyJSON;
			_isPlaying = false;

			_inkStory = new Story(_storyJSON.text);
			foreach (StoryBehaviour b in _behaviours) b.OnStoryLoaded(this);

			if (playImmediately) {
				PlayStory();
			}
		}

		public void PlayStory() {
			if (_isPlaying) return;
			_isPlaying = true;
			ContinueStory(false);
		}

		void ContinueStory(bool useSeparator) {
			foreach (StoryBehaviour b in _behaviours) b.OnStoryBeforeContinue();

			int textIndex = 0;
			while (_inkStory.canContinue && textIndex < 500) {
				string text = _inkStory.Continue().Trim();

				if (textIndex == 0) {
					foreach (StoryBehaviour b in _behaviours) b.OnStoryText(text, useSeparator, true);
				} else {
					foreach (StoryBehaviour b in _behaviours) b.OnStoryText(text, false, false);
				}

				textIndex++;
			}

			foreach (StoryBehaviour b in _behaviours) b.OnStoryAfterContinue();

			if (_inkStory.currentChoices.Count > 0) {
				for (int i = 0; i < _inkStory.currentChoices.Count; i++) {
					string choiceText = _inkStory.currentChoices[i].text.Trim();
					foreach (StoryBehaviour b in _behaviours) b.OnStoryOption(choiceText, i);
				}

			} else {
				foreach (StoryBehaviour b in _behaviours) b.OnStoryEnd();
			}
		}

		public void ChooseOption(int index) {
			if (_isPlaying) {
				_inkStory.ChooseChoiceIndex(index);
				ContinueStory(true);
			} else {
				Debug.LogWarning("Cannot choose option, story is not playing");
			}
		}
	}
}