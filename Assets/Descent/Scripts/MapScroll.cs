using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class MapScroll : MonoBehaviour {
	[SerializeField] float _transitionDuration = .5f;
	[SerializeField] Ease _easeType;
	[SerializeField] bool _verticalScrolling;
	[SerializeField] bool _horizontalScrolling;

	[SerializeField] List<RectTransform> _zones;

	[SerializeField] int showZone = -1;
	[SerializeField, HideInInspector] int currentZone;

	void OnValidate() {
		if (Application.isPlaying) {
			showZone = Mathf.Clamp(showZone, -1, _zones.Count-1);
			ShowZone(showZone, true);
		}
	}

	void Awake() {
		//hide all zones
		foreach (RectTransform zone in _zones) {
			zone.GetComponent<Image>().color = new Color(1,1,1,0);
		}
		currentZone = -1;
	}

	[ContextMenu("Show Current")]
	void ShowCurrentZone() {
		ShowZone(currentZone);
	}

	public void ShowZone(int index, bool ignoreErrors = false) {
		if (index > -1 && index < _zones.Count && _zones[index] != null) {
			//scroll to the zone's position
			RectTransform zone = _zones[index];
			Vector3 target = new Vector3(
				_horizontalScrolling ? -zone.localPosition.x : transform.localPosition.x,
				_verticalScrolling ? -zone.localPosition.y : transform.localPosition.y,
				0f);
			DOTween.Kill(this);
			transform.DOLocalMove(target, _transitionDuration).SetId(this)
				.SetEase(_easeType);
			//hide the currently showing zone
			if (currentZone > -1) {
				DOTween.Kill(_zones[currentZone]);
				_zones[currentZone].GetComponent<Image>().DOFade(0f, _transitionDuration).SetId(_zones[currentZone]);
			}
			//show the new current zone
			currentZone = index;
			DOTween.Kill(zone);
			zone.GetComponent<Image>().DOFade(1f, _transitionDuration).SetId(zone);

		} else if (!ignoreErrors) {
			Debug.LogError("Could not show zone "+index.ToString());
			//reset to a default position
			if (currentZone > -1) {
				_zones[currentZone].GetComponent<Image>().color = new Color(1,1,1,0);
			}
			currentZone = -1;
			transform.localPosition = Vector3.zero;
		}
	}
}
