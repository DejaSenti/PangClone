using UnityEngine;

public class Ball : PoolableObject
{
    public BallCollisionEvent BallCollisionEvent;

    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RB;

    public float BaseSize;

    public int Size;
    public float Radius { get => Mathf.Pow(2, Size) * BaseSize; }

    public Color Color;

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
        var velocity = spawnPoint.InitialDirection.normalized * spawnPoint.InitialBallSpeed;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ComponentTagNames.PROJECTILE)
        {
            BallCollisionEvent.Invoke(this);
        }
    }
}