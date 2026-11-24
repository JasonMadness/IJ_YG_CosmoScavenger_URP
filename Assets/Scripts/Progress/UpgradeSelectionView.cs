using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectionView : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Experience _experience;
    [SerializeField] private UpgradeCatalog _catalog;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button[] _optionButtons;
    [SerializeField] private Text[] _optionLabels;

    public event System.Action PauseNeeded;
    public event System.Action ResumeNeeded;

    private readonly List<PlayerUpgrade> _options = new();
    private int _pendingSelections;

    private void Awake()
    {
        for (int i = 0; i < _optionButtons.Length; i++)
        {
            int index = i;
            _optionButtons[i].onClick.AddListener(() => Select(index));
        }

        _panel.SetActive(false);
    }

    private void OnEnable()
    {
        _experience.LevelIncreased += HandleLevelIncreased;
    }

    private void OnDisable()
    {
        _experience.LevelIncreased -= HandleLevelIncreased;
    }

    private void HandleLevelIncreased()
    {
        _pendingSelections++;

        if (_panel.activeSelf)
            return;

        ShowOptions();
    }

    private void ShowOptions()
    {
        FillOptions();

        for (int i = 0; i < _optionButtons.Length; i++)
        {
            bool hasOption = i < _options.Count;
            _optionButtons[i].gameObject.SetActive(hasOption);

            if (hasOption)
                _optionLabels[i].text = _options[i].Title + "\n" + _options[i].Description;
        }

        _panel.SetActive(true);
        PauseNeeded?.Invoke();
    }

    private void FillOptions()
    {
        _options.Clear();

        List<PlayerUpgrade> available = new(_catalog.Upgrades);
        int count = Mathf.Min(_optionButtons.Length, available.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, available.Count);
            _options.Add(available[index]);
            available.RemoveAt(index);
        }
    }

    private void Select(int index)
    {
        if (index < 0 || index >= _options.Count)
            return;

        _options[index].Apply(_player);
        _pendingSelections--;

        if (_pendingSelections > 0)
        {
            ShowOptions();
            return;
        }

        _panel.SetActive(false);
        ResumeNeeded?.Invoke();
    }
}
