using UnityEngine;

public abstract class BaseBuster: MonoBehaviour
{
    public virtual void Initialize(BustersStats stats)
    { }
    public abstract void OnCollected(Player player);
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") is false)
            return;
        
        OnCollected(other.GetComponent<Player>());
        gameObject.SetActive(false);
    }
}