using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    // Singleton pattern
    public static AudioManager Instance { get; private set; }

    private StudioEventEmitter emitter;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);

            emitter = GetComponent<StudioEventEmitter>();
        }
    }

    public void PlaySound(string soundName)
    {
        Debug.Log("Playing sound: " + soundName);
        emitter.Play();
    }

    public void PlayMusic(string soundName)
    {
        Debug.Log("Playing music: " + soundName);
    }
}
