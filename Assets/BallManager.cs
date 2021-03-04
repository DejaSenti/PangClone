using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject level;
    [SerializeField]
    private float ballSpeedOnSplit;

    private ObjectPool<Ball> pool;
    private BallSpawnPoint[] ballSpawnPoints;

    public void Initialize(GameObject level)
    {
        this.level = level;

        ballSpawnPoints = level.GetComponentsInChildren<BallSpawnPoint>();

        if (pool == null)
        {
            pool = new ObjectPool<Ball>(ball);
        }

        int poolSize = 0;

        foreach(BallSpawnPoint spawnPoint in ballSpawnPoints)
        {
            var maxNumFromPoint = Mathf.CeilToInt(Mathf.Pow(BallData.NUM_SPLIT_BALLS, spawnPoint.InitialBallSize));
            poolSize += maxNumFromPoint;
        }

        pool.Initialize(poolSize);
    }

    public void StartLevel()
    {
        foreach(BallSpawnPoint spawnPoint in ballSpawnPoints)
        {
            var ball = pool.Acquire();

            if (ball == null)
            {
                Debug.LogError("Can't start level with pool fully occupied!");
                return;
            }

            ball.Initialize(spawnPoint);

            ball.BallCollisionEvent.AddListener(OnBallCollision);
        }
    }

    private void OnBallCollision(Ball ball)
    {
        pool.Release(ball);
        ball.BallCollisionEvent.RemoveListener(OnBallCollision);

        if (ball.Size > 1)
        {
            var newSize = ball.Size - 1;
            var newColor = ball.Color;
            var newPosition = ball.transform.position;

            for (int i = 0; i < BallData.NUM_SPLIT_BALLS; i++)
            {
                var newBall = pool.Acquire();

                if (newBall == null)
                {
                    Debug.LogError("Pool was too small to contain level properly.");
                    return;
                }

                var newVelocity = BallData.DIRECTION_VECTORS[i].normalized * ballSpeedOnSplit;

                newBall.Initialize(newSize, newColor, newPosition, newVelocity);

                newBall.BallCollisionEvent.AddListener(OnBallCollision);
            }
        }
    }

    [ContextMenu("Test Level")]
    public void TestLevel()
    {
        Initialize(level);
        StartLevel();
    }
}
