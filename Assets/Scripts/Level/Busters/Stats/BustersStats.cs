using UnityEngine;

[CreateAssetMenu(menuName = "Static data/Create Busters stats", fileName = "Busters stats")]
public class BustersStats: ScriptableObject
{
    public HealthBusterStats healthBusterStats;
    public InvulnerabilityBusterStats invulnerabilityBusterStats;

}