using UnityEngine;

[CreateAssetMenu(fileName = "SnakeTextures", menuName = "Scriptable Objects/SnakeTextures")]
public class SnakeTextures : ScriptableObject
{
    [SerializeField] public Sprite SnakeHead;
    [SerializeField] public Sprite SnakeBodyTail;
    [SerializeField] public Sprite SnakeBodyCorner;
    [SerializeField] public Sprite SnakeBodyStraight;
}
