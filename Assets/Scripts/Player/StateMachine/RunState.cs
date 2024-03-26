using UnityEngine;

public class RunState : State
{

    [SerializeField] private float speed;
    [SerializeField] private bool smoothActivated;
    [SerializeField] [Range(0f,0.3f)] private float movementSmooth = 0.3f;
    
    private Vector3 _velocity = Vector3.zero;
    
    public override void Enter()
    {
        StateAnimator.Play("Run");
    }

    public override void Do()
    {
        float horizontalMovement = StateInput.XInput * speed;
        Vector3 targetVelocity = new Vector2(horizontalMovement, Body.velocity.y);
        Body.velocity = smoothActivated ? Vector3.SmoothDamp(Body.velocity, targetVelocity, ref _velocity, movementSmooth) : targetVelocity;    
        
        if (!StateInput.Grounded) IsComplete = true;
    }
    
}
