using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : State
{

    private float _verticalVelocity;
    private bool _isFalling;
    private bool _lastIsFalling;
    
    public override void Enter()
    {
        _verticalVelocity = Body.linearVelocity.y;
        if (_verticalVelocity < -0.1f) _isFalling = true;
        StateAnimator.Play(_isFalling ? "Fall" : "Jump");

        _lastIsFalling = _isFalling;
    }

    public override void Do()
    {
        if (StateInput.Grounded)
        {
            IsComplete = true;
            return;
        }

        _verticalVelocity = Body.linearVelocity.y;
        _isFalling = _verticalVelocity <= 0;

        if (_isFalling && _lastIsFalling != _isFalling) StateAnimator.Play("TrJumpFall");
        else if (!_isFalling) StateAnimator.Play("Jump");
        
        _lastIsFalling = _isFalling;
    }

}
