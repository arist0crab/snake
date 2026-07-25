using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : MonoBehaviour
{
    private static PauseWindow instanse;

    private void Awake()
    {
        instanse = this;

        Button resumeButton = transform.Find("resumeButton").GetComponent<Button>();
        Button backToMenuButton = transform.Find("mainMenuButton").GetComponent<Button>();

        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);

        Hide();
    }

    public static void ShowStatic()
    {
        instanse.Show();
    }

    public static void HideStatic()
    {
        instanse.Hide();
    }

    private void OnResumeButtonClicked()
    {
        GameHandler.ResumeGame();
    }

    private void OnBackToMenuButtonClicked()
    {
        Loader.Load(Loader.Scene.MainMenu);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
