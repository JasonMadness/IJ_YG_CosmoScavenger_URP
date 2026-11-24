using UnityEngine;
using UnityEngine.UI;

public class ExperienceView : MonoBehaviour
{
    [SerializeField] private Experience _experience;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Text _levelLabel;

    private void Start()
    {
        OnExperienceChanged();
    }

    public void OnExperienceChanged()
    {
        _progressSlider.value = _experience.Progress;
        _levelLabel.text = "Level " + _experience.Level;
    }
}
