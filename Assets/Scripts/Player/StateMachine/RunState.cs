using UnityEngine;
using FMODUnity;

public class RunState : State
{
    private StudioEventEmitter _stepSoundEmitter;

    private void Awake()
    {
        _stepSoundEmitter = GetComponent<StudioEventEmitter>();
    }

    public override void Enter()
    {
        StateAnimator.Play("Run");
        StartRunSound();
    }

    public override void Do()
    {
        if (!StateInput.Grounded) IsComplete = true;
    }

    public override void Exit()
    {
        StopRunSound();
    }

    // Sounds

    private void PlayStepSound()
    {
        _stepSoundEmitter.Play();
    }

    public void StartRunSound()
    {
        InvokeRepeating("PlayStepSound", 0.01f, 0.25f);
    }

    public void StopRunSound()
    {
        CancelInvoke("PlayStepSound");
    }

}
