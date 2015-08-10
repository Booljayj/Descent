using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryPanel : MonoBehaviour {
	public Text textBox;
	public Image panel;
	
	public void SetPanel(string text, Color color) {
		textBox.text = text;
		panel.color = color;
	}
}