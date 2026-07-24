using System;
using UnityEngine;

public class SnakeInputManager : MonoBehaviour
{
    public static SnakeInputManager Instance { get; private set; } 

    public event Action<Vector3Int> OnMoveInput;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            OnMoveInput?.Invoke(Snake.SnakeMoveDirection.Top);

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            OnMoveInput?.Invoke(Snake.SnakeMoveDirection.Down);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            OnMoveInput?.Invoke(Snake.SnakeMoveDirection.Left);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            OnMoveInput?.Invoke(Snake.SnakeMoveDirection.Right);
    }
}
