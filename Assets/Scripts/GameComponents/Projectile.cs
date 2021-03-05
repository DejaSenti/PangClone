using UnityEngine;

public abstract class Projectile : PoolableObject
{
    public ProjectileDestroyedEvent DestroyedEvent;
    public PlayerID OwnerID;

    private void Awake()
    {
        if (DestroyedEvent == null)
        {
            DestroyedEvent = new ProjectileDestroyedEvent();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyedEvent.Invoke(this);
    }
}
