using UnityEngine;
using UnityEngine.Assertions;
using Ink.Runtime;

public class StoryReader : MonoBehaviour {
	[SerializeField] TextAsset _story;

	[Space(10)]
	[SerializeField] RectTransform _panelContainer;
	[SerializeField] RectTransform _panelFooter;
	[SerializeField] StoryButton[] _choiceButtons;
	[SerializeField] CanvasGroup _group;

	[Space(10)]
	[SerializeField] RectTransform _separatorPrefab;
	[SerializeField] StoryPanel _panelPrefab;

	Story _inkStory;
	public Story Story {get {return _inkStory;}}

	public bool interactable {
		get {return _group.interactable;}
		set {_group.interactable = value;}
	}

	public event System.Action<Story> OnStoryLoaded;
	public event System.Action OnBeforeContinue;
	public event System.Action OnAfterContinue;

	void Start() {
		_inkStory = new Story(_story.text);
		if (OnStoryLoaded != null) OnStoryLoaded(_inkStory);

		ClearButtons();
		ContinueStory(false);
	}

	void OnValidate() {
		Assert.IsTrue(_story, "Must set a story for the reader to read");
	}

	void ContinueStory(bool useSeparator) {
		if (OnBeforeContinue != null) OnBeforeContinue();

		//create a separator from the previous stream of text
		if (useSeparator && _separatorPrefab) {
			RectTransform separator = Instantiate<RectTransform>(_separatorPrefab);
			separator.SetParent(_panelContainer, false);
		}

		//create panels for each paragraph of text from the story
		while (_inkStory.canContinue) {
			StoryPanel panel = Instantiate<StoryPanel>(_panelPrefab);
			panel.transform.SetParent(_panelContainer, false);

			panel.SetPanel(_inkStory.Continue().Trim());
		}
		//advance the footer to the bottom of the feed
		_panelFooter.SetAsLastSibling();

		//set up the buttons for the next part of the story
		ClearButtons();
		for (int i = 0; i < _inkStory.currentChoices.Count && i < _choiceButtons.Length; i++) {
			int choiceIndex = i;
			_choiceButtons[i].Setup(_inkStory.currentChoices[i].text.Trim(), delegate{ChooseOption(choiceIndex);});
		}

		if (OnAfterContinue != null) OnAfterContinue();
	}

	void ChooseOption(int index) {
		_inkStory.ChooseChoiceIndex(index);
		ContinueStory(true);
	}

	void ClearButtons() {
		for (int i = 0; i < _choiceButtons.Length; i++) {
			_choiceButtons[i].Clear();
		}
	}
}