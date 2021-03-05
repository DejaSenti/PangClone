using UnityEngine;

public class Gun : MonoBehaviour
{
    public int BulletLimit;

    [SerializeField]
    private GameObject projectilePrefab;

    private ObjectPool<Projectile> pool;

    private void Awake()
    {
        if (projectilePrefab.GetComponentInChildren<Projectile>() == null)
        {
            Debug.LogError("Assigned projectile doesn't have valid projectile component.");
        }
    }

    public void Initialize()
    {
        if (pool == null)
        {
            pool = new ObjectPool<Projectile>(projectilePrefab);
        }

        pool.Initialize(BulletLimit);
    }

    public void Shoot(PlayerID playerID)
    {
        var projectile = pool.Acquire();

        if (projectile == null)
            return;

        projectile.OwnerID = playerID;

        projectile.transform.localPosition = transform.position;

        projectile.DestroyedEvent.AddListener(OnProjectileDestroyed);
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