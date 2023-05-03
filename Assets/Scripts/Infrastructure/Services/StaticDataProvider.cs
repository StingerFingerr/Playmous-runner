using UnityEngine;
[CreateAssetMenu(menuName = "Static data/Create static data provider")]
public class StaticDataProvider: ScriptableObject, IService
{
    public LevelConfigData levelConfigData;
    public AssetReferences assetReferences;
    public PlayerStats playerStats;
    public BustersStats bustersStats;
}