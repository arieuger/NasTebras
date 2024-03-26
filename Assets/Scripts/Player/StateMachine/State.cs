using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool IsComplete { get; protected set; }

    protected float StartTime;
    public float StateTime => Time.time - StartTime;
    
    protected Rigidbody2D Body;
    protected Animator StateAnimator;
    protected PlayerMovement StateInput;
    

    public virtual void Enter() { }
    public virtual void Do() { }
    public virtual void FixedDo() { }
    public virtual void Exit() { }
    
    public void Setup(Rigidbody2D _body, Animator _animator, PlayerMovement _playerMovement) {
        Body = _body;
        StateAnimator = _animator;
        StateInput = _playerMovement;
    }

    public void Initialise() {
        IsComplete = false;
        StartTime = Time.time;
    }
}
