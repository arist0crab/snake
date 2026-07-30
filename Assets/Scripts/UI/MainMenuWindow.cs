using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    private void Awake()
    {
        Transform bgTransform = transform.Find("BackgroundImage");
        Button playButton = bgTransform.Find("playButton").GetComponent<Button>();
        Button quitButton = bgTransform.Find("quitButton").GetComponent<Button>();

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
