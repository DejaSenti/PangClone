using UnityEngine;
using UnityEngine.Events;

public class BallManager : MonoBehaviour
{
    public static ScoreEvent ScoreEvent;

    public UnityEvent AllBallsDestroyedEvent;

    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private float ballSpeedOnSplit;

    private ObjectPool<Ball> pool;
    private BallSpawnPoint[] ballSpawnPoints;

    private void Awake()
    {
        if (AllBallsDestroyedEvent == null)
        {
            AllBallsDestroyedEvent = new UnityEvent();
        }

        if (ScoreEvent == null)
        {
            ScoreEvent = new ScoreEvent();
        }
    }

    public void InitializeLevel(GameObject level)
    {
        ballSpawnPoints = level.GetComponentsInChildren<BallSpawnPoint>();

        if (pool == null)
        {
            pool = new ObjectPool<Ball>(ballPrefab);
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
        if (pool.ActiveCount > 0)
        {
            pool.ReleaseAll();
        }

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

    private void OnBallCollision(Ball ball, PlayerID playerID)
    {
        pool.Release(ball);
        ball.BallCollisionEvent.RemoveListener(OnBallCollision);

        ScoreEvent.Invoke(ball, playerID);

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

        if (pool.ActiveCount == 0)
        {
            AllBallsDestroyedEvent.Invoke();
        }
    }

    public void EndLevel()
    {
        var poolObjects = pool.GetAllPooledObjects();
        foreach (var ball in poolObjects)
        {
            ball.BallCollisionEvent.RemoveListener(OnBallCollision);
        }

        pool.Terminate();
    }
}