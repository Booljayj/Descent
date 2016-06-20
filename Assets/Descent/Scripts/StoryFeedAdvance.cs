using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoryFeedAdvance : MonoBehaviour {
	[SerializeField] StoryReader _reader;
	[SerializeField] ScrollRectStepped _scrollRect;
	[SerializeField] float _feedDuration;

	float _scrollSteps;

	void Awake() {
		_reader.OnAfterContinue += ScrollToBottom;
	}

	void ScrollToBottom() {
		Canvas.ForceUpdateCanvases();
		DOTween.Kill(this);
		DOTween.To(GetPos, SetPos, _scrollRect.content.sizeDelta.y, _feedDuration).SetId(this).SetEase(Ease.Linear);
	}

	float GetPos() {
		return _scrollRect.content.anchoredPosition.y;
	}

	void SetPos(float f) {
		_scrollRect.SetAnchoredPosition(new Vector2(0,f));
	}

	float StepwiseEase(float time, float duration, float overshootOrAmplitude, float period) {
		return Mathf.Ceil((time / duration)*_scrollSteps)/_scrollSteps;
	}
}
