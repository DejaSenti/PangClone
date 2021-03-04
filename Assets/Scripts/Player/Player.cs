using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerID PlayerID;

    public Rigidbody2D RB;

    public float Acceleration;
    public float MaxSpeed;

    public float DecelerationMultiplier;

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
