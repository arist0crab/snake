using UnityEngine;

public class GameAssets : MonoBehaviour
{
    [SerializeField] public Sprite snakeHeadSprite;

    public static GameAssets i;

    private void Awake()
    {
        i = this;
    }
}
