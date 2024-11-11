using UnityEngine;

public class DropPoint : MonoBehaviour
{
    public static Transform ItemDropPosition { get; private set; }

    private void Start()
    {
        ItemDropPosition = transform;
    }
}
