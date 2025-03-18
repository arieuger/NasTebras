using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AirState : State
{

    private float _verticalVelocity;
    private bool _isFalling;
    private bool _lastIsFalling;

    private StudioEventEmitter _jumpSoundEmitter;

    private void Awake()
    {
        _jumpSoundEmitter = GetComponent<StudioEventEmitter>();
    }

    public override void Enter()
    {
        _verticalVelocity = Body.linearVelocity.y;
        if (_verticalVelocity < -0.1f) _isFalling = true;
        StateAnimator.Play(_isFalling ? "Fall" : "Jump");

        _lastIsFalling = _isFalling;

        _jumpSoundEmitter.Play();
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
