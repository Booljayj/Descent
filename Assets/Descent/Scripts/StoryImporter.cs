using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StoryImporter {
	public static Hashtable ImportInklewriter(string raw) {
		Hashtable data = new Hashtable(3);
		List<Story.Snippet> snippets = new List<Story.Snippet>();
		
		JSONObject root = new JSONObject(raw);
		JSONObject stitches = root["data"]["stitches"];
		
		data.Add("title", root["title"].str);
		data.Add("start", root["data"]["initial"].str);
		
		foreach (string name in stitches.keys) {
			Story.Snippet snippet = new Story.Snippet();
			List<Story.Flag> flags = new List<Story.Flag>();
			List<Story.Option> options = new List<Story.Option>();
			List<Story.Condition> conditions = new List<Story.Condition>();
			
			JSONObject node = stitches[name]["content"];
			
			foreach (JSONObject content in node.list) {
				if (content.type == JSONObject.Type.STRING) {
					snippet.text = Sanitize(content.str);
					
				} else if (content.HasField("flagName")) {
					//Debug.Log("Flag: "+content["flagName"].str);
					flags.Add(ProcessInkleFlag(content["flagName"].str));
					
				} else if (content.HasField("option")) {
					List<Story.Condition> optConditions = new List<Story.Condition>();
					
					if (!content["ifConditions"].IsNull) {
						foreach (JSONObject cond in content["ifConditions"].list) {
							//Debug.Log("Condition: "+cond["ifCondition"].str);
							optConditions.Add(ProcessInkleCondition(cond["ifCondition"].str, false));
						}
					}
					
					if (!content["notIfConditions"].IsNull) {
						foreach (JSONObject cond in content["notIfConditions"].list) {
							//Debug.Log("Not Condition: "+cond["notIfCondition"].str);
							optConditions.Add(ProcessInkleCondition(cond["notIfCondition"].str, true));
						}
					}
					
					options.Add(new Story.Option(Sanitize(content["option"].str),
					                             content["linkPath"].str,
					                             optConditions.ToArray()));
					
				} else if (content.HasField("divert")) {
					snippet.passthrough = content["divert"].str;
					
				} else if (content.HasField("ifCondition")) {
					//Debug.Log("Snippet Condition: "+content["ifCondition"].str);
					conditions.Add(ProcessInkleCondition(content["ifCondition"].str, false));
					
				} else if (content.HasField("notIfCondition")) {
					//Debug.Log("Snippet Not Condition: "+content["notIfCondition"].str);
					conditions.Add(ProcessInkleCondition(content["notIfCondition"].str, true));
				}
			}
			
			snippet.id = name;
			snippet.flags = flags.ToArray();
			snippet.options = options.ToArray();
			snippet.conditions = conditions.ToArray();
			snippets.Add(snippet);
		}

		data.Add("snippets", snippets);
		
		return data;
	}
	
	public static Story.Condition ProcessInkleCondition(string input, bool invert) {
		string name;
		Story.Condition.Comparison comp = Story.Condition.Comparison.Exists;
		int val = 0;
		
		string[] strings = input.Split(new char[]{'=','<','>'}, System.StringSplitOptions.RemoveEmptyEntries);
		name = strings[0].Trim();
		if (strings.Length > 1) {
			int.TryParse(strings[1].Trim(), out val);
			
			if (input.Contains(" = ")) {
				if (invert) comp = Story.Condition.Comparison.NotEqual;
				else comp = Story.Condition.Comparison.Equal;
				
			} else if (input.Contains(" > ")) {
				if (invert) comp = Story.Condition.Comparison.LessEqual;
				else comp = Story.Condition.Comparison.Greater;
				
			} else if (input.Contains(" < ")) {
				if (invert) comp = Story.Condition.Comparison.GreaterEqual;
				else comp = Story.Condition.Comparison.Less;
				
			} else if (input.Contains(" >= ")) {
				if (invert) comp = Story.Condition.Comparison.Less;
				else comp = Story.Condition.Comparison.GreaterEqual;
				
			} else if (input.Contains(" <= ")) {
				if (invert) comp = Story.Condition.Comparison.Greater;
				else comp = Story.Condition.Comparison.LessEqual;
			}
			
		} else {
			if (invert) comp = Story.Condition.Comparison.NotExists;
			else comp = Story.Condition.Comparison.Exists;
		}
		
		return new Story.Condition(name, comp, val);
	}

	public static Story.Flag ProcessInkleFlag(string input) {
		string name;
		Story.Flag.Operation oper = Story.Flag.Operation.Set;
		int val = 1;

		string[] strings = input.Split(new char[]{'=','+','-','*','/'}, System.StringSplitOptions.RemoveEmptyEntries);
		name = strings[0].Trim();

		if (strings.Length > 1) {
			int.TryParse(strings[1].Trim(), out val);

			if (input.Contains("+")) {
				oper = Story.Flag.Operation.Add;
			} else if (input.Contains("-")) {
				oper = Story.Flag.Operation.Subtract;
			} else if (input.Contains("*")) {
				oper = Story.Flag.Operation.Multiply;
			} else if (input.Contains("/")) {
				oper = Story.Flag.Operation.Divide;
			} else {
				oper = Story.Flag.Operation.Set;
			}
		}

		return new Story.Flag(name, oper, val);
	}

	public static string Sanitize(string input) {
		return input.Replace("\\\"", "\"").
			Replace("/=","<i>").
			Replace("=/","</i>").
			Replace("*-","<b>").
			Replace("-*", "</b>");
	}
}