using UnityEngine;
using System.Collections.Generic;

namespace StoryReading {
	public class StoryFeed : StoryBehaviour {
		[SerializeField] RectTransform _panelContainer;
		[SerializeField] RectTransform _panelFooter;

		[Space(10)]
		[SerializeField] RectTransform _separatorPrefab;
		[SerializeField] StoryPanel _panelPrefab;

		[SerializeField, HideInInspector] List<GameObject> _feedObjects;

		void Awake() {
			_feedObjects = new List<GameObject>();
		}

		public override void OnStoryLoaded(StoryReader reader) {
			if (_feedObjects.Count > 0) {
				foreach (GameObject go in _feedObjects) {
					Destroy(go);
				}
				_feedObjects.Clear();
			}
		}

		public override void OnStoryAfterContinue() {
			_panelFooter.SetAsLastSibling();
		}

		public override void OnStoryText(string text, bool useSeparator, bool isFirstText) {
			if (useSeparator) {
				RectTransform separator = Instantiate<RectTransform>(_separatorPrefab);
				AddToFeed(separator);
			}

			StoryPanel panel = Instantiate<StoryPanel>(_panelPrefab);
			AddToFeed(panel.transform);
			panel.SetPanel(text);
		}

		void AddToFeed(Transform t) {
			_feedObjects.Add(t.gameObject);
			t.SetParent(_panelContainer, false);
		}
	}
}
