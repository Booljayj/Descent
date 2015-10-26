using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Story : ScriptableObject {
	public string title;
	public string start;

	public List<Snippet> snippets = new List<Snippet>();
	Dictionary<string, Snippet> sdict;

	void CreateDictionary() {
		sdict = new Dictionary<string, Snippet>();
		foreach (Snippet snippet in snippets) {
			sdict.Add(snippet.id, snippet);
		}
	}

	public Passage GetPassage(string id, StoryContext context) {
		string currentSnippet = id;
		string fullText = "";
		List<Passage.Link> fullLinks = new List<Passage.Link>();

		if (sdict == null) {
			CreateDictionary();
		}

		while (!string.IsNullOrEmpty(currentSnippet)) {
			Snippet snippet = snippets.Find((s) => s.id == currentSnippet);

			//evalutate flags first, to determine new context in this snippet
			foreach (Flag flag in snippet.flags) {
				flag.Evaluate(context);
			}

			//evaluate conditions on snippet to determine if text should be shown
			bool showText = true;
			foreach (Condition condition in snippet.conditions) {
				showText = condition.Evaluate(context);
				if (!showText) break;
			}

			if (showText) {
				if (string.IsNullOrEmpty(fullText)) {
					fullText = ""+snippet.text;
				} else {
					fullText += "\n\n"+snippet.text;
				}
			}

			//evaluate all options, determine if they should be shown
			foreach (Option option in snippet.options) {
				bool showOption = true;
				foreach (Condition condition in option.conditions) {
					showOption = condition.Evaluate(context);
					if (!showOption) break;
				}

				if (showOption) {
					fullLinks.Add(new Passage.Link(option.text, option.link));
				}
			}

			//passthrough if found
			currentSnippet = snippet.passthrough;
		}

		return new Passage(fullText, fullLinks.ToArray());
	}

	[System.Serializable]
	public class Snippet {
		public string name {get {return id;}}
		//content
		public string id;
		[TextArea] public string text;
		public Option[] options;
		public Flag[] flags;

		//behavior
		public string passthrough;
		public Condition[] conditions;
		public bool end;
	}

	[System.Serializable]
	public class Option {
		public string name {get {return text;}}
		//content
		public string text;
		public string link;

		//behavior
		public Condition[] conditions;

		public Option(string text, string link, Condition[] conditions) {
			this.text = text;
			this.link = link;
			this.conditions = conditions;
		}
	}

	[System.Serializable]
	public struct Flag {
		public enum Operation {
			Set,
			Add,
			Subtract,
			Multiply,
			Divide
		}

		public string name;
		public int value;
		public Operation operation;

		public Flag(string name, Operation operation, int value) {
			this.name = name;
			this.operation = operation;
			this.value = value;
		}

		public void Evaluate(StoryContext context) {
			switch (operation) {

			case Operation.Add:
				context.Set(name, context.Get(name) + value);
				break;

			case Operation.Subtract:
				context.Set(name, context.Get(name) - value);
				break;

			case Operation.Multiply:
				context.Set(name, context.Get(name) * value);
				break;

			case Operation.Divide:
				if (value == 0) {
					Debug.LogError("Attempted to divide by 0, ignoring");
				} else {
					context.Set(name, context.Get(name) / value);
				}
				break;

			default:
				context.Set(name, value);
				break;
			}
		}
	}

	[System.Serializable]
	public struct Condition {
		public enum Comparison {
			Exists, NotExists,
			Equal, NotEqual,
			Greater, Less,
			GreaterEqual, LessEqual
		};

		public string name;
		public int value;
		public Comparison comparison;

		public Condition(string name, Comparison comparison, int value) {
			this.name = name;
			this.comparison = comparison;
			this.value = value;
		}

		public bool Evaluate(StoryContext context) {
			switch (comparison) {

			case Comparison.Exists:
				if (context.Contains(name)) return true;
				else return false;

			case Comparison.NotExists:
				if (!context.Contains(name)) return true;
				else return false;

			case Comparison.Equal:
				if (context.Contains(name) && value == context.Get(name))
					return true;
				else return false;

			case Comparison.NotEqual:
				if (context.Contains(name) && value != context.Get(name))
					return true;
				else return false;

			case Comparison.Greater:
				if (context.Contains(name) && value < context.Get(name))
					return true;
				else return false;

			case Comparison.Less:
				if (context.Contains(name) && value > context.Get(name))
					return true;
				else return false;

			case Comparison.GreaterEqual:
				if (context.Contains(name) && value <= context.Get(name))
					return true;
				else return false;

			case Comparison.LessEqual:
				if (context.Contains(name) && value >= context.Get(name))
					return true;
				else return false;

			default:
				return true;
			}
		}
	}
}