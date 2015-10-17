using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MapScroll : MonoBehaviour {
	public string variableName = "Area";
	public float transitionDuration = .5f;
	public AnimationCurve easingCurve;
	public bool verticalScrolling;
	public bool horizontalScrolling;

	public List<RectTransform> zones;

	Animator currentZone;

	public void ContextChange(string name, int val) {
		if (name.Equals(variableName)) {
			if (val < zones.Count && zones[val] != null) {
				if (currentZone) currentZone.SetBool("Show", false);
				currentZone = zones[val].GetComponent<Animator>();
				if (currentZone) currentZone.SetBool("Show", true);

				StopAllCoroutines();
				StartCoroutine(MoveToArea(zones[val]));
			}
		}
	}

	IEnumerator MoveToArea(RectTransform t) {
		float elapsedTime = 0;
		Vector3 origin = transform.localPosition;
		Vector3 target = new Vector3(
			horizontalScrolling? -t.localPosition.x : transform.localPosition.x,
			verticalScrolling? -t.localPosition.y : transform.localPosition.y,
			0f
		);

		while (elapsedTime < transitionDuration) {
			transform.localPosition = Vector3.Lerp(origin, target, easingCurve.Evaluate(elapsedTime/transitionDuration));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.localPosition = target;
	}
}
