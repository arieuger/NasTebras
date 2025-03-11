using UnityEngine;

public class RunState : State
{

    public override void Enter()
    {
        StateAnimator.Play("Run");
        AudioManager.Instance.PlaySound("Run");
    }

    public override void Do()
    {


        if (!StateInput.Grounded) IsComplete = true;
    }

}
