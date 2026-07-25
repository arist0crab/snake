using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;

    [SerializeField] private Snake snake;
    public bool IsGameRun = true;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        IsGameRun = true;
        ApplicationInputManager.Instance.OnPausePressed += HandlePause;
    }

    void OnDisable()
    {
        ApplicationInputManager.Instance.OnPausePressed -= HandlePause;
    }

    // TODO
    void Update()
    {
        if (!snake.IsAlive)
        {
            GameOverWindow.ShowStatic();
            return;
        }
    }

    private void HandlePause()
    {
        if (IsGameRun) PauseGame();
        else ResumeGame();
    }

    public void PauseGame()
    {
        PauseWindow.ShowStatic();
        IsGameRun = false;
    }

    public void ResumeGame()
    {
        PauseWindow.HideStatic();
        IsGameRun = true;
    }
}
