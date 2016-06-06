using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StoryButton : MonoBehaviour {
	[SerializeField] Button _button;
	[SerializeField] Text _text;

	public void Setup(string choiceText, UnityAction choiceDelegate) {
		gameObject.SetActive(true);
		_button.onClick.AddListener(choiceDelegate);
		_text.text = choiceText;
	}

	public void Clear() {
		gameObject.SetActive(false);
		_button.onClick.RemoveAllListeners();
		_text.text = "Deactivated Button";
	}
}
