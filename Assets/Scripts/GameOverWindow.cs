using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private static GameOverWindow instance;

    private void Awake()
    {
        instance = this;

        Button retryButton = transform.Find("retryButton").GetComponent<Button>();
        Button backToMenuButton = transform.Find("mainMenuButton").GetComponent<Button>();

        retryButton.onClick.AddListener(OnRetryButtonClicked);
        backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);

        Hide();
    }

    private void OnRetryButtonClicked()
    {
        Loader.Load(Loader.Scene.GameScene);
    }

    private void OnBackToMenuButtonClicked()
    {
        Loader.Load(Loader.Scene.MainMenu);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowStatic()
    {
        instance.Show();
    }
}
