using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryPanel : MonoBehaviour {
	public Text textBox;
	
	public void SetPanel(string text) {
		textBox.text = text;
	}
}