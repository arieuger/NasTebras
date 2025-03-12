using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    // Singleton pattern
    public static AudioManager Instance { get; private set; }

    // Materials
    public const string MATERIAL_CONCRETE = "concrete";

    private StudioEventEmitter stepEmitter;

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

            stepEmitter = GetComponent<StudioEventEmitter>();
        }
    }

    // Movement sounds

    public void StartRun(string material = MATERIAL_CONCRETE)
    {
        InvokeRepeating("PlayStepSound", 0.01f, 0.25f);
    }

    public void StopRun()
    {
        CancelInvoke("PlayStepSound");
    }

    private void PlayStepSound()
    {
        stepEmitter.Play();
    }

    // Music

    public void PlayMusic(string soundName)
    {
        Debug.Log("Playing music: " + soundName);
    }
}
