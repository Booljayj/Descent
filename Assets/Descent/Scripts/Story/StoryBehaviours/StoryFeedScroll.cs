using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace StoryReading {
	public class StoryFeedScroll : StoryBehaviour {
		[SerializeField] ScrollRectStepped _scrollRect;
		[SerializeField] CanvasGroup _buttonGroup;
		[SerializeField] float _feedDuration;
		[SerializeField] AudioSource _feedSound;

		public override void OnStoryAfterContinue() {
			Canvas.ForceUpdateCanvases();
			DOTween.Kill(this);
			DOTween.To(GetPos, SetPos, _scrollRect.content.sizeDelta.y, _feedDuration).SetId(this)
				.OnStart(OnStart)
				.OnComplete(OnComplete)
				.SetEase(Ease.Linear);
		}

		float GetPos() {
			return _scrollRect.content.anchoredPosition.y;
		}

		void SetPos(float f) {
			_scrollRect.SetAnchoredPosition(new Vector2(0,f));
		}

		void OnStart() {
			_scrollRect.onPositionChanged.AddListener(
				delegate{_feedSound.PlayOneShot(_feedSound.clip);});
			_buttonGroup.interactable = false;
		}

		void OnComplete() {
			_scrollRect.onPositionChanged.RemoveAllListeners();
			_buttonGroup.interactable = true;
		}
	}
}
