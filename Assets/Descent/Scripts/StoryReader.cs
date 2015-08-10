using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StoryReader : MonoBehaviour {
	[SerializeField] Story story;
	[SerializeField] StoryContext context;
	
	[SerializeField] string currentId;
	[SerializeField, HideInInspector] Passage currentPassage;

	[Space(10)]
	public RectTransform scrollbox;
	public ScrollRect scroller;
	public StoryPanel panelPrefab;
	public Color passageColor;
	public Color choiceColor;

	[Space(10)]
	public Button[] buttons;

	void Start() {
		if (string.IsNullOrEmpty(currentId)) {
			currentId = story.start;
		}

		//======= this part all needs to be moved around
		currentPassage = story.GetPassage(currentId, context);

		//create a passage panel to show in the scroll
		StoryPanel passage = Instantiate<StoryPanel>(panelPrefab);
		passage.SetPanel(currentPassage.text, passageColor);
		
		//show the choice and the new passage
		passage.transform.SetParent(scrollbox);
		
		for (int i = 0; i < buttons.GetLength(0); i++) {
			if (i < currentPassage.links.GetLength(0)) {
				buttons[i].gameObject.SetActive(true);
				buttons[i].GetComponentInChildren<Text>().text = currentPassage.links[i].text;
			} else {
				buttons[i].gameObject.SetActive(false);
			}
		}
	}
	
	void OnValidate() {
		if (story) {
			if (string.IsNullOrEmpty(currentId)) {
				currentPassage = story.GetPassage(story.start, context);
			} else {
				currentPassage = story.GetPassage(currentId, context);
			}
		} else {
			currentPassage = null;
		}
	}
	
	public void ResetStory(bool keepContext) {
		if (!keepContext) {
			context.Clear();
		}
		currentId = story.start;
		currentPassage = story.GetPassage(currentId, context);

		foreach (Transform t in scrollbox) {
			Destroy(t.gameObject);
		}
	}

	public void ChooseOption(int index) {
		//create a choice panel to show in the scroll
		StoryPanel choice = Instantiate<StoryPanel>(panelPrefab);
		choice.SetPanel(currentPassage.links[index].text, choiceColor);

		//get the new passage
		currentId = currentPassage.links[index].link;
		currentPassage = story.GetPassage(currentId, context);

		//create a passage panel to show in the scroll
		StoryPanel passage = Instantiate<StoryPanel>(panelPrefab);
		passage.SetPanel(currentPassage.text, passageColor);

		//show the choice and the new passage
		choice.transform.SetParent(scrollbox);
		passage.transform.SetParent(scrollbox);

		//scroll to the bottom
		Canvas.ForceUpdateCanvases();
		scroller.normalizedPosition = Vector2.zero;

		for (int i = 0; i < buttons.GetLength(0); i++) {
			if (i < currentPassage.links.GetLength(0)) {
				buttons[i].gameObject.SetActive(true);
				buttons[i].GetComponentInChildren<Text>().text = currentPassage.links[i].text;
			} else {
				buttons[i].gameObject.SetActive(false);
			}
		}
	}
}