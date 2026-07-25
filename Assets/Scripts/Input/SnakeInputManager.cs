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
            OnMoveInput?.Invoke(Vector3Int.up);

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            OnMoveInput?.Invoke(Vector3Int.down);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            OnMoveInput?.Invoke(Vector3Int.left);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            OnMoveInput?.Invoke(Vector3Int.right);
    }
}
