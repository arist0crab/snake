using System;
using UnityEngine;

public class ApplicationInputManager : MonoBehaviour
{
    public static ApplicationInputManager Instance { get; private set; }

    public event Action OnPausePressed;
    
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        HandleInput();
    }
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnPausePressed?.Invoke();
    }
}
