using UnityEngine;

public class GameAssets : MonoBehaviour
{
    [SerializeField] public Sprite snakeHeadSprite;
    [SerializeField] public Sprite foodSprite;

    public static GameAssets i;

    private void Awake()
    {
        i = this;
    }
}
