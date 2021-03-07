using UnityEngine;

public class Ball : PoolableObject, IScorable
{
    public BallCollisionEvent BallCollisionEvent;

    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RB;

    public float BaseSize;

    public int Size { get; private set; }
    public float Radius { get => Mathf.Pow(2, Size) * BaseSize; }

    public Color Color { get; private set; }

    private void Awake()
    {
        if (BallCollisionEvent == null)
        {
            BallCollisionEvent = new BallCollisionEvent();
        }
    }

    public void Initialize(BallSpawnPoint spawnPoint)
    {
        var size = spawnPoint.InitialBallSize;
        var color = spawnPoint.InitialBallColour;
        var position = spawnPoint.transform.position;
        var velocity = spawnPoint.InitialDirection.normalized * BallData.BALL_SPEED_ON_SPAWN;

        Initialize(size, color, position, velocity);
    }

    public void Initialize(int size, Color color, Vector2 position, Vector2 velocity)
    {
        Size = size;
        var localScale = new Vector3(Radius, Radius, 1);
        transform.localScale = localScale;

        Color = color;
        SpriteRenderer.color = Color;

        transform.position = position;

        RB.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == ComponentTagNames.PROJECTILE)
        {
            var projectile = collider.GetComponent<Projectile>();
            BallCollisionEvent.Invoke(this, projectile.OwnerID);
        }
    }
}