using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        GameScene,
        Loading,
        MainMenu
    }

    public static void Load(Scene scene)
    {
        if (scene == Scene.GameScene)
            SceneManager.LoadScene(Scene.Loading.ToString());

        if (scene == Scene.MainMenu)
            SceneManager.LoadScene(Scene.MainMenu.ToString());
    }
}
