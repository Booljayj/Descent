using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemView : MonoBehaviour {
	[SerializeField] string _itemName;
	public string itemName {get {return _itemName;}}

    [Space(10f)]
	[SerializeField] Image _image;
	[SerializeField] LayoutElement _layoutElement;
	[Space(10f)]
	[SerializeField] float _transitionDuration;
	[SerializeField] Vector2 _preferredSizeShown;
	[SerializeField] Vector2 _preferredSizeHidden;

	void Reset() {
		_layoutElement = GetComponent<LayoutElement>();
		_image = GetComponentInChildren<Image>();
	}

	void OnValidate() {
		name = string.Format("Item-{0}", string.IsNullOrEmpty(_itemName) ? "null" : _itemName);
		_layoutElement.preferredWidth = _preferredSizeShown.x;
		_layoutElement.preferredHeight = _preferredSizeShown.y;
	}

	void Awake() {
		_layoutElement.preferredWidth = _preferredSizeHidden.x;
		_layoutElement.preferredHeight = _preferredSizeHidden.y;
		_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
	}

	public void Setup(string name, Sprite sprite) {
		_itemName = name;
		_image.sprite = sprite;
	}

	[ContextMenu("Show")]
	public void Show() {
		DOTween.Kill(this);
		DOTween.Sequence().SetId(this)
			.OnStart(OnStartShow)
			.Append(_layoutElement.DOPreferredSize(_preferredSizeShown, _transitionDuration).SetEase(Ease.OutQuad))
			.Join(_image.DOFade(1f, _transitionDuration));
	}

	[ContextMenu("Hide")]
	public void Hide() {
		DOTween.Kill(this);
		DOTween.Sequence().SetId(this)
			.OnComplete(OnCompleteHide)
			.Append(_layoutElement.DOPreferredSize(_preferredSizeHidden, _transitionDuration).SetEase(Ease.InQuad))
			.Join(_image.DOFade(0f, _transitionDuration));
	}

	void OnStartShow() {
		gameObject.SetActive(true);
	}

	void OnCompleteHide() {
		gameObject.SetActive(false);
	}
}
