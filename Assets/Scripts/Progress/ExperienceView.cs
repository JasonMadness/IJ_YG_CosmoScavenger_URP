using UnityEngine;
using UnityEngine.UI;

public class ExperienceView : MonoBehaviour
{
    [SerializeField] private Experience _experience;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Text _levelLabel;

    private void OnEnable()
    {
        _experience.Changed += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        _experience.Changed -= Refresh;
    }

    private void Refresh()
    {
        _progressSlider.value = _experience.Progress;
        _levelLabel.text = "Level " + _experience.Level;
    }
}
