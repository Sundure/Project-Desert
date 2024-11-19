using UnityEngine;

public class PickubleAudioSource : MonoBehaviour
{
    public static AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }
}
