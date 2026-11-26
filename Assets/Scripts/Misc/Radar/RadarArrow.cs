using UnityEngine;
using UnityEngine.UI;

public sealed class RadarArrow : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Color _color = new Color(1f, 0.16f, 0.16f, 1f);

    private RectTransform _rect;

    private void Awake()
    {
        _rect = (RectTransform)transform;

        if (_icon == null)
            _icon = GetComponent<Image>();

        if (_icon == null)
            return;

        _icon.sprite = RadarSprites.Arrow;
        _icon.color = _color;
        _icon.raycastTarget = false;
    }

    public void Show(Vector2 direction, float distance)
    {
        if (_rect == null)
            return;

        _rect.anchoredPosition = direction * distance;
        _rect.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
    }
}
