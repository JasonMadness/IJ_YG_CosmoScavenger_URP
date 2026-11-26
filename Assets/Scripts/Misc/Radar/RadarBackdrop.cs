using UnityEngine;
using UnityEngine.UI;

public sealed class RadarBackdrop : MonoBehaviour
{
    [SerializeField] private Image _image;

    private void Awake()
    {
        if (_image == null)
            _image = GetComponent<Image>();

        if (_image == null)
            return;

        _image.sprite = RadarSprites.Circle;
        _image.raycastTarget = false;
    }
}
