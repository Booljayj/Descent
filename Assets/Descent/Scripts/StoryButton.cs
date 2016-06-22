using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StoryButton : MonoBehaviour {
	[SerializeField] Button _button;
	[SerializeField] Text _text;

	public void Setup(string choiceText, UnityAction choiceDelegate) {
		_button.interactable = true;
		if (choiceDelegate != null) _button.onClick.AddListener(choiceDelegate);
		_text.gameObject.SetActive(true);
		_text.text = choiceText;
	}

	public void Clear() {
		_button.interactable = false;
		_button.onClick.RemoveAllListeners();
		_text.gameObject.SetActive(false);
		_text.text = "Deactivated Button";
	}
}
