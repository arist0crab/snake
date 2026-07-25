using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    private void Awake()
    {
        Button playButton = transform.Find("playButton").GetComponent<Button>();
        Button quitButton = transform.Find("quitButton").GetComponent<Button>();

        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        Loader.Load(Loader.Scene.GameScene);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
