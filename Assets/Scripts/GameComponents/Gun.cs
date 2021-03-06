using UnityEngine;

[RequireComponent(typeof(Timer))]
public class Gun : MonoBehaviour
{
    public int BulletLimit;
    public float CooldownTime;

    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Timer cooldownTimer;

    private ObjectPool<Projectile> pool;

    private PlayerID ownerID;

    private void Awake()
    {
        if (projectilePrefab.GetComponentInChildren<Projectile>() == null)
        {
            Debug.LogError("Assigned projectile doesn't have valid projectile component.");
        }
    }

    public void Initialize(PlayerID ownerID)
    {
        if (pool == null)
        {
            pool = new ObjectPool<Projectile>(projectilePrefab);
        }

        pool.Initialize(BulletLimit);

        this.ownerID = ownerID;
    }

    public void Shoot()
    {
        if (cooldownTimer.enabled)
            return;

        var projectile = pool.Acquire();

        if (projectile == null)
            return;

        projectile.OwnerID = ownerID;

        projectile.transform.localPosition = transform.position;

        projectile.DestroyedEvent.AddListener(OnProjectileDestroyed);

        cooldownTimer.StartTimer(CooldownTime);
    }

    private void OnProjectileDestroyed(Projectile projectile)
    {
        projectile.DestroyedEvent.RemoveListener(OnProjectileDestroyed);
        pool.Release(projectile);
    }

    public void Reset()
    {
        pool.ReleaseAll();
    }

    public void Terminate()
    {
        var allProjectiles = pool.GetAllPooledObjects();
        foreach(Projectile projectile in allProjectiles)
        {
            projectile.DestroyedEvent.RemoveListener(OnProjectileDestroyed);
        }

        pool.Terminate();
    }
}