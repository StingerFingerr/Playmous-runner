using System;
using UnityEngine;

public class HealthBuster: BaseBuster
{
    private HealthBusterStats _stats;

    public override void Initialize(BustersStats stats) => 
        _stats = stats.healthBusterStats;

    public override void OnCollected(Player player) =>
        player.AddHealth(_stats.health);

    private void Update() => 
        transform.Rotate(Vector3.up*10*Time.deltaTime);
}