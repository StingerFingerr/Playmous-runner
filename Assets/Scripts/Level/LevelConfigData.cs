using UnityEngine;

[CreateAssetMenu(menuName = "Static data/Create Level config data", fileName = "Level config data")]
public class LevelConfigData: ScriptableObject
{
    public int blocksAmount = 50;
    [Range(0,100)] public int busterChance = 10;

}