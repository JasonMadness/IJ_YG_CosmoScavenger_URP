using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Pause _pause;
    [SerializeField] private Experience _experience;
    [SerializeField] private UpgradeSelectionView _upgradeSelectionView;
    [SerializeField] private ExperienceView _experienceView;

    private void OnEnable()
    {
        if (!HasRequiredReferences())
            return;

        _upgradeSelectionView.PauseNeeded += _pause.OnPauseNeeded;
        _upgradeSelectionView.ResumeNeeded += _pause.OnResumeNeeded;
        _experience.LevelIncreased += _upgradeSelectionView.OnLevelIncreased;
        _experience.Changed += _experienceView.OnExperienceChanged;
    }

    private void OnDisable()
    {
        if (_pause == null || _experience == null || _upgradeSelectionView == null || _experienceView == null)
            return;

        _upgradeSelectionView.PauseNeeded -= _pause.OnPauseNeeded;
        _upgradeSelectionView.ResumeNeeded -= _pause.OnResumeNeeded;
        _experience.LevelIncreased -= _upgradeSelectionView.OnLevelIncreased;
        _experience.Changed -= _experienceView.OnExperienceChanged;
    }

    private bool HasRequiredReferences()
    {
        bool hasAll = true;

        if (_pause == null)
        {
            Debug.LogError($"[Bootstrap] {nameof(_pause)} не назначен в Inspector.", this);
            hasAll = false;
        }

        if (_experience == null)
        {
            Debug.LogError($"[Bootstrap] {nameof(_experience)} не назначен в Inspector.", this);
            hasAll = false;
        }

        if (_upgradeSelectionView == null)
        {
            Debug.LogError($"[Bootstrap] {nameof(_upgradeSelectionView)} не назначен в Inspector.", this);
            hasAll = false;
        }

        if (_experienceView == null)
        {
            Debug.LogError($"[Bootstrap] {nameof(_experienceView)} не назначен в Inspector.", this);
            hasAll = false;
        }

        return hasAll;
    }
}
