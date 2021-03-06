﻿using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    public static int LevelCount;

    [SerializeField]
    private GameController gameController;

    private void Awake()
    {
        LevelCount = BuildHelpers.GetLevelCount();
    }

    private void Start()
    {
        gameController.Initialize(2);
        gameController.StartNewGame();
    }
}