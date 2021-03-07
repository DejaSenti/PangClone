using UnityEngine;

public static class BallData
{
    public const int NUM_SPLIT_BALLS = 2;
    // The array MUST be the same size as the integer!
    public static readonly Vector2[] DIRECTION_VECTORS = new Vector2[] { new Vector2(1, 1), new Vector2(-1, 1) };
           
    public const float BALL_SPEED_ON_SPAWN = 3;
}
