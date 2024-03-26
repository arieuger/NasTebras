public class IdleState : State
{
    
    public override void Enter()
    {
        StateAnimator.Play("Idle");
    }

    public override void Do()
    {
        if (!StateInput.Grounded) IsComplete = true;
    }
    
}
