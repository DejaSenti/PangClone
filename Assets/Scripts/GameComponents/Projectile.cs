using UnityEngine;

public abstract class Projectile : PoolableObject
{
    public ProjectileDestroyedEvent DestroyedEvent;

    private void Awake()
    {
        if (DestroyedEvent == null)
        {
            DestroyedEvent = new ProjectileDestroyedEvent();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyedEvent.Invoke(this);
    }
}
