using UnityEngine;
using System.Collections;

[System.Serializable]
public class Passage {
	public string text;
	public Link[] links;
	
	public Passage(string text, Link[] links) {
		this.text = text;
		this.links = links;
	}
	
	public struct Link {
		public string text;
		public string link;

		public Link(string text, string link) {
			this.text = text;
			this.link = link;
		}
	}
}