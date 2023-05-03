using UnityEngine;
using UnityEngine.UI;

public class ReportDataView: MonoBehaviour
{
    [SerializeField] private Text holeBlocksText;
    [SerializeField] private Text fenceBlocksText;
    [SerializeField] private Text sawBlocksText;

    public void ShowReport(ReportData reportData)
    {
        holeBlocksText.text = $"Hole blocks: {reportData.HoleBlocksAmount}";
        fenceBlocksText.text = $"Fence blocks: {reportData.FenceBlocksAmount}";
        sawBlocksText.text = $"Saw blocks: {reportData.SawBlockAmount}";
    }
}