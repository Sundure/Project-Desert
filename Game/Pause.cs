using UnityEngine;

public class Pause : MonoBehaviour
{
    public static void UnpauseGame()
    {
        Time.timeScale = 1;
    }
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
}
