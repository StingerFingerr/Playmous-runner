using System;
using UnityEngine;

public class LoseWindow: MonoBehaviour
{
    [SerializeField] private ReportDataView reportDataView;
    
    private Action _nextLevel;
    private Action _continueLevel;

    public void Show(Action nextLevel, Action continueLevel, ReportData reportData)
    {
        _nextLevel = nextLevel;
        _continueLevel = continueLevel;
        
        reportDataView.ShowReport(reportData);
        gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        _nextLevel.Invoke();
    }

    public void Continue()
    {
        _continueLevel.Invoke();
    }
}