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

    public override void Activate()
    {
        gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyedEvent.Invoke(this);
    }
}
