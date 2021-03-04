using UnityEngine;

public class DefaultProjectile : Projectile
{
    [SerializeField]
    private float speed;

    public override void Activate()
    {
        base.Activate();

        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        var deltaPosition = Vector3.up * speed * Time.deltaTime;
        transform.localScale += deltaPosition;
        transform.localPosition += deltaPosition / 2;
    }
}