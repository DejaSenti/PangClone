using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool IsDestructible;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsDestructible && collision.tag == ComponentTagNames.PROJECTILE)
        {
            Destroy(gameObject);
        }
    }
}
