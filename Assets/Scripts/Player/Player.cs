using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public PlayerID PlayerID;

    public PlayerHitEvent HitEvent;

    public Rigidbody2D RB;
    public Gun Gun;

    public float Acceleration;
    public float MaxSpeed;

    public float DecelerationMultiplier;

    private void Awake()
    {
        if (HitEvent == null)
        {
            HitEvent = new PlayerHitEvent();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == ComponentTagNames.BALL)
        {
            HitEvent.Invoke(PlayerID);
        }
    }

    public void Move(MovementDirection direction)
    {
        float addedSpeedX;

        switch (direction)
        {
            case MovementDirection.Left:
                if (RB.velocity.x < -MaxSpeed)
                    return;

                addedSpeedX = -Acceleration * Time.deltaTime;
                break;

            case MovementDirection.Right:
                if (RB.velocity.x > MaxSpeed)
                    return;

                addedSpeedX = Acceleration * Time.deltaTime;
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
        var newVelocityX = RB.velocity.x * DecelerationMultiplier;
        RB.velocity = new Vector2(newVelocityX, RB.velocity.y);
    }
}