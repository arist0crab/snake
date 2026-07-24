using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private static GameOverWindow instance;

    private void Awake()
    {
        instance = this;

        Button retryButton = transform.Find("retryButton").GetComponent<Button>();
        retryButton.onClick.AddListener(OnRetryButtonClicked);

        Hide();
    }

    private void OnRetryButtonClicked()
    {
        Loader.Load();
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
