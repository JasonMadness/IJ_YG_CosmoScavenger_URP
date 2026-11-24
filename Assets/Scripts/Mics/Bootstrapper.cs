using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Pause _pause;
    [SerializeField] private UpgradeSelectionView _upgradeSelectionView;

    private void OnEnable()
    {
        if (_pause == null)
        {
            Debug.LogError($"[Bootstrap] {nameof(_pause)} не назначен в Inspector.", this);
            return;
        }

        if (_upgradeSelectionView == null)
        {
            Debug.LogError($"[Bootstrap] {nameof(_upgradeSelectionView)} не назначен в Inspector.", this);
            return;
        }

        _upgradeSelectionView.PauseNeeded += _pause.OnPauseNeeded;
        _upgradeSelectionView.ResumeNeeded += _pause.OnResumeNeeded;
    }

    private void OnDisable()
    {
        if (_pause == null || _upgradeSelectionView == null)
            return;

        _upgradeSelectionView.PauseNeeded -= _pause.OnPauseNeeded;
        _upgradeSelectionView.ResumeNeeded -= _pause.OnResumeNeeded;
    }
}
