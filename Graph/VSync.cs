using UnityEngine;

public class VSync : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 1;

        DontDestroyOnLoad(gameObject);
    }
}
