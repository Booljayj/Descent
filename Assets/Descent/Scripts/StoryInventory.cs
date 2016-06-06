using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

public class StoryInventory : MonoBehaviour {
	const string DECLARE_METHOD_NAME = "DeclareItem";

	[SerializeField] StoryReader _reader;
	[SerializeField] string _imagePath;
	[SerializeField] RectTransform _viewContainer;
	[SerializeField] ItemView _viewPrefab;

	[Space(10f)]
	[SerializeField] List<ItemView> _itemViews;

	void Awake() {
		_reader.OnStoryLoaded += BindInventoryFunctions;
	}

	/// <summary>Attach an external method to the ink story, which can be called using DECLARE_METHOD_NAME</summary>
	void BindInventoryFunctions(Story story) {
		story.BindExternalFunction<string>(DECLARE_METHOD_NAME, new System.Action<string>(DeclareItem));
		//observe pre-declared items
		foreach (ItemView view in _itemViews) ObserveVariable(view.itemName);
	}

	/// <summary>Declare a new variable that should be used as an item</summary>
	void DeclareItem(string name) {
		//check if the item has already been declared, and escape if it has
		foreach (ItemView view in _itemViews)
			if (view.itemName == name) return;
		//create a view for it
		ItemView newView = Instantiate<ItemView>(_viewPrefab);
		newView.transform.SetParent(_viewContainer, false);
		//setup the view with a name and image
		newView.Setup(name, Resources.Load<Sprite>(_imagePath+name));
		//add the view to the list for later retrieval
		_itemViews.Add(newView);
		//start observing the variable
		ObserveVariable(name);
	}

	/// <summary>Observe the given variable name</summary>
	void ObserveVariable(string name) {
		//begin observing changes in this variable
		_reader.Story.ObserveVariable(name, VariableChanged);
	}

	/// <summary>Called when a variable with the given name is set to a new value</summary>
	void VariableChanged(string name, object value) {
		for (int i = 0; i < _itemViews.Count; i++) {
			if (_itemViews[i].itemName == name) {
				int v = (int)value;
				if (v > 0) _itemViews[i].Show();
				else _itemViews[i].Hide();
			}
		}
	}
}
