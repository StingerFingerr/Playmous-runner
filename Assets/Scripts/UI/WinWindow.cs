using System;
using UnityEngine;

public class WinWindow: MonoBehaviour
{
    [SerializeField] private ReportDataView reportDataView;
    private Action nextLevel;
    
    public void Show(Action nextLevel, ReportData reportData)
    {
        this.nextLevel = nextLevel;
        
        reportDataView.ShowReport(reportData);
        gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        nextLevel.Invoke();
    }
        
}