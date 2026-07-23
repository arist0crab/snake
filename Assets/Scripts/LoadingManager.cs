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
        yield return new WaitForSeconds(2);

        gameLoading.allowSceneActivation = true;
    }
}
