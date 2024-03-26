using UnityEngine;

public class RunState : State
{
    
    public override void Enter()
    {
        StateAnimator.Play("Run");
    }

    public override void Do()
    {
            
        
        if (!StateInput.Grounded) IsComplete = true;
    }
    
}
