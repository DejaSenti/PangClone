using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerID ID { get; private set; }

    public PlayerHitEvent HitEvent;

    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float decelerationMultiplier;

    [SerializeField]
    private Gun gun;
    [SerializeField]
    private Rigidbody2D RB;
    [SerializeField]
    private SpriteRenderer sprite;

    private void Awake()
    {
        if (HitEvent == null)
        {
            HitEvent = new PlayerHitEvent();
        }
    }

    public void Initialize(PlayerID playerID, Color color)
    {
        ID = playerID;
        sprite.color = color;

        gun.Initialize(playerID);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == ComponentTagNames.BALL)
        {
            HitEvent.Invoke(ID);
        }
    }

    public void Move(MovementDirection direction)
    {
        float addedSpeedX;

        switch (direction)
        {
            case MovementDirection.Left:
                if (RB.velocity.x < -maxSpeed)
                    return;

                addedSpeedX = -acceleration * Time.deltaTime;
                break;

            case MovementDirection.Right:
                if (RB.velocity.x > maxSpeed)
                    return;

                addedSpeedX = acceleration * Time.deltaTime;
                break;

            default:
                Decelerate();
                return;
        }

        var addedVelocity = new Vector2(addedSpeedX, 0);

        RB.velocity += addedVelocity;
    }

    private void Decelerate()
    {
        var newVelocityX = RB.velocity.x * decelerationMultiplier;
        RB.velocity = new Vector2(newVelocityX, RB.velocity.y);
    }

    public void Shoot()
    {
        gun.Shoot();
    }

    public void Deactivate()
    {
        gun.Reset();
        gameObject.SetActive(false);
    }

    public void Terminate()
    {
        Destroy(gameObject);
    }
}