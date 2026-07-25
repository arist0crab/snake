using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        AsyncOperation gameLoading = SceneManager.LoadSceneAsync(Loader.Scene.GameScene.ToString());
        gameLoading.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(2f);

        gameLoading.allowSceneActivation = true;
    }
}
