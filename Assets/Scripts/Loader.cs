using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        GameScene,
        Loading
    }

    public static void Load()
    {
        SceneManager.LoadScene(Scene.Loading.ToString());
    }
}
