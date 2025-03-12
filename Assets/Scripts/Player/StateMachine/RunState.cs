using UnityEngine;

public class RunState : State
{

    public override void Enter()
    {
        StateAnimator.Play("Run");
        AudioManager.Instance.StartRun();
    }

    public override void Do()
    {
        if (!StateInput.Grounded) IsComplete = true;
    }

    public override void Exit()
    {
        AudioManager.Instance.StopRun();
    }

}
