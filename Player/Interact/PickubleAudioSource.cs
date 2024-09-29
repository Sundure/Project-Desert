using UnityEngine;

public class PickubleAudioSource : MonoBehaviour
{
    public static AudioSource AudioSource;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }
}
