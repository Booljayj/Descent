using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class ItemView : MonoBehaviour {
	public string itemName;

	Animator animator;

	void Awake() {
		gameObject.SetActive(false);
		animator = GetComponent<Animator>();
	}

	public void ContextChange(string key, int value) {
		if (key.Equals(itemName)) {
			if (value > 0) Show();
			else Hide();
		}
	}

	void Show() {
		gameObject.SetActive(true);

		if (animator) {
			animator.SetBool("Show", true); //should be true by default, ensure it
		}

		transform.SetAsFirstSibling();
	}

	void Hide() {
		if (animator) {
			animator.SetBool("Show", false);
		} else {
			gameObject.SetActive(false);
		}
	}
}
